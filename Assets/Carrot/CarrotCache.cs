#region License
/* Carrot -- Copyright (C) 2012 GoCarrot Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion
#if UNITY_IPHONE || UNITY_ANDROID
#   define CACHE_ENABLED
#endif

#region References
using System;
using CarrotInc.MiniJSON;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

/// @cond hide_from_doxygen
public class CarrotCache : IDisposable
{
    public bool InstallMetricSent
    {
        get;
        private set;
    }

    public long InstallDate
    {
        get;
        private set;
    }

    public void markInstallMetricSent()
    {
#if CACHE_ENABLED
        sqlite3_exec(mDBPtr, kInstallTableMetricSentSQL, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
#endif
        this.InstallMetricSent = true;
    }

    public CarrotCache()
    {
#if CACHE_ENABLED
        if(sqlite3_open(Application.persistentDataPath + "/carrot.db", out mDBPtr) != SQLITE_OK)
        {
            Debug.Log("Failed to create Carrot request cache. Error: " + sqlite3_errmsg(mDBPtr));
            mDBPtr = IntPtr.Zero;
        }
        else
        {
            if(sqlite3_exec(mDBPtr, kCacheCreateSQL, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != SQLITE_OK)
            {
                Debug.Log("Failed to create Carrot request cache. Error: " + sqlite3_errmsg(mDBPtr));
                sqlite3_close(mDBPtr);
                mDBPtr = IntPtr.Zero;
            }
            else if(sqlite3_exec(mDBPtr, kInstallTableCreateSQL, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != SQLITE_OK)
            {
                Debug.Log("Failed to create Carrot request cache. Error: " + sqlite3_errmsg(mDBPtr));
                sqlite3_close(mDBPtr);
                mDBPtr = IntPtr.Zero;
            }
            else
            {
                IntPtr sqlStatement = IntPtr.Zero;
                double cachedInstallDate = 0.0;
                if(sqlite3_prepare_v2(mDBPtr, kInstallTableReadSQL, -1, out sqlStatement, IntPtr.Zero) == SQLITE_OK)
                {
                    while(sqlite3_step(sqlStatement) == SQLITE_ROW)
                    {
                        cachedInstallDate = sqlite3_column_double(sqlStatement, 0);
                        this.InstallMetricSent = sqlite3_column_int(sqlStatement, 1) > 0 ? true : false;
                    }
                }
                sqlite3_finalize(sqlStatement);

                if(cachedInstallDate < 1.0)
                {
                    cachedInstallDate = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    string sql = string.Format(kInstallTableUpdateSQL, cachedInstallDate);
                    sqlite3_exec(mDBPtr, sql, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
                this.InstallDate = (long)cachedInstallDate;
            }
        }
#else
        mCachedRequests = new List<CachedRequest>();
#endif
    }

    public CachedRequest CacheRequest(Carrot.ServiceType serviceType, string endpoint, Dictionary<string, object> parameters)
    {
        CachedRequest ret = new CachedRequest();
        ret.ServiceType = serviceType;
        ret.Endpoint = endpoint;
        ret.Parameters = parameters;
        ret.RequestDate = (long)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        ret.RequestId = System.Guid.NewGuid().ToString();
        ret.Retries = 0;

#if CACHE_ENABLED
        IntPtr sqlStatement = IntPtr.Zero;
        string sql = string.Format(kCacheInsertSQL, ret.ServiceType, ret.Endpoint, Json.Serialize(ret.Parameters),
                                   ret.RequestId, ret.RequestDate, ret.Retries);
        lock(this)
        {
            if(sqlite3_prepare_v2(mDBPtr, sql, -1, out sqlStatement, IntPtr.Zero) == SQLITE_OK)
            {
                if(sqlite3_step(sqlStatement) == SQLITE_DONE)
                {
                    ret.Cache = this;
                    ret.CacheId = sqlite3_last_insert_rowid(mDBPtr);
                }
                else
                {
                    Debug.Log("Failed to write request to Carrot cache. Error: " + sqlite3_errmsg(mDBPtr));
                }
            }
            else
            {
                Debug.Log("Failed to write request to Carrot cache. Error: " + sqlite3_errmsg(mDBPtr));
            }
        }
        sqlite3_finalize(sqlStatement);
#else
        lock(this)
        {
            ret.Cache = this;
            mCachedRequests.Add(ret);
        }
#endif
        return ret;
    }

    public List<CachedRequest> RequestsInCache(Carrot.AuthStatus authStatus)
    {
        List<CachedRequest> cachedRequests = new List<CachedRequest>();
#if CACHE_ENABLED
        IntPtr sqlStatement = IntPtr.Zero;
        lock(this)
        {
            string sql = string.Format(kCacheReadSQL, (int)authStatus);
            if(sqlite3_prepare_v2(mDBPtr, sql, -1, out sqlStatement, IntPtr.Zero) == SQLITE_OK)
            {
                while(sqlite3_step(sqlStatement) == SQLITE_ROW)
                {
                    CachedRequest request = new CachedRequest();
                    request.CacheId = sqlite3_column_int64(sqlStatement, 0);
                    request.ServiceType = (Carrot.ServiceType)sqlite3_column_int(sqlStatement, 1);
                    request.Endpoint = sqlite3_column_text(sqlStatement, 2);
                    request.Parameters = Json.Deserialize(sqlite3_column_text(sqlStatement, 3)) as Dictionary<string, object>;
                    request.RequestId = sqlite3_column_text(sqlStatement, 4);
                    request.RequestDate = (long)sqlite3_column_double(sqlStatement, 5);
                    request.Retries = sqlite3_column_int(sqlStatement, 6);
                    request.Cache = this;
                    cachedRequests.Add(request);
                }
            }
            else
            {
                Debug.Log("Failed to load requests from Carrot cache. Error: " + sqlite3_errmsg(mDBPtr));
            }
        }
        sqlite3_finalize(sqlStatement);
#else
        lock(this)
        {
            foreach(CarrotCache.CachedRequest crequest in mCachedRequests)
            {
                if((int)crequest.ServiceType <= (int)authStatus)
                {
                    //Debug.Log("Somethingsomething: " + crequest);
                    cachedRequests.Add(crequest);
                }
            }
        }
#endif
        return cachedRequests;
    }

    public class CachedRequest
    {
        public Dictionary<string, object> Parameters
        {
            get;
            internal set;
        }

        public Carrot.ServiceType ServiceType
        {
            get;
            internal set;
        }

        public string Endpoint
        {
            get;
            internal set;
        }

        public string RequestId
        {
            get;
            internal set;
        }

        public long RequestDate
        {
            get;
            internal set;
        }

        public int Retries
        {
            get;
            internal set;
        }

        internal long CacheId
        {
            get;
            set;
        }

        internal CarrotCache Cache
        {
            get;
            set;
        }

        internal CachedRequest() {}

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2} - {3}: {4}", this.CacheId, this.ServiceType, this.RequestId, this.Endpoint, this.Parameters);
        }

        public bool RemoveFromCache()
        {
            bool ret = true;
#if CACHE_ENABLED
            IntPtr sqlStatement = IntPtr.Zero;
            string sql = string.Format(kCacheDeleteSQL, this.CacheId);
            lock(this.Cache)
            {
                if(sqlite3_prepare_v2(this.Cache.mDBPtr, sql, -1, out sqlStatement, IntPtr.Zero) == SQLITE_OK)
                {
                    if(sqlite3_step(sqlStatement) != SQLITE_DONE)
                    {
                        Debug.Log("Failed to remove request from Carrot cache. Error: " + sqlite3_errmsg(this.Cache.mDBPtr));
                        ret = false;
                    }
                }
                else
                {
                    Debug.Log("Failed to remove request from Carrot cache. Error: " + sqlite3_errmsg(this.Cache.mDBPtr));
                    ret = false;
                }
            }
            sqlite3_finalize(sqlStatement);
#else
            lock(this.Cache)
            {
                this.Cache.mCachedRequests.Remove(this);
            }
#endif
            return ret;
        }

        public bool AddRetryInCache()
        {
            bool ret = true;
#if CACHE_ENABLED
            IntPtr sqlStatement = IntPtr.Zero;
            string sql = string.Format(kCacheUpdateSQL, this.Retries + 1, this.CacheId);
            lock(Cache)
            {
                if(sqlite3_prepare_v2(this.Cache.mDBPtr, sql, -1, out sqlStatement, IntPtr.Zero) == SQLITE_OK)
                {
                    if(sqlite3_step(sqlStatement) != SQLITE_DONE)
                    {
                        Debug.Log("Failed to add retry to request in Carrot cache. Error: " + sqlite3_errmsg(this.Cache.mDBPtr));
                        ret = false;
                    }
                }
                else
                {
                    Debug.Log("Failed to add retry to request in Carrot cache. Error: " + sqlite3_errmsg(this.Cache.mDBPtr));
                    ret = false;
                }
            }
            sqlite3_finalize(sqlStatement);
#endif
            return ret;
        }
    }

    #region IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!mIsDisposed)
        {
            if(disposing)
            {
#if CACHE_ENABLED
                lock(this)
                {
                    sqlite3_close(mDBPtr);
                    mDBPtr = IntPtr.Zero;
                }
#endif
            }
        }
        mIsDisposed = true;
    }

    ~CarrotCache()
    {
        Dispose(false);
    }
    #endregion

#region DLL Imports
#if CACHE_ENABLED
    private const int SQLITE_OK = 0;
    private const int SQLITE_ROW = 100;
    private const int SQLITE_DONE = 101;

    private const string kCacheCreateSQL = "CREATE TABLE IF NOT EXISTS cache(request_servicetype INTEGER, request_endpoint TEXT, request_payload TEXT, request_id TEXT, request_date REAL, retry_count INTEGER)";
    private const string kCacheReadSQL = "SELECT rowid, request_servicetype, request_endpoint, request_payload, request_id, request_date, retry_count FROM cache WHERE request_servicetype<={0} ORDER BY retry_count LIMIT 10";
    private const string kCacheInsertSQL = "INSERT INTO cache (request_servicetype, request_endpoint, request_payload, request_id, request_date, retry_count) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
    private const string kCacheUpdateSQL = "UPDATE cache SET retry_count='{0}' WHERE rowid='{1}'";
    private const string kCacheDeleteSQL = "DELETE FROM cache WHERE rowid='{0}'";

    private const string kInstallTableCreateSQL = "CREATE TABLE IF NOT EXISTS install_tracking(install_date REAL, metric_sent INTEGER)";
    private const string kInstallTableReadSQL = "SELECT MAX(install_date), metric_sent FROM install_tracking";
    private const string kInstallTableUpdateSQL = "INSERT INTO install_tracking (install_date, metric_sent) VALUES ({0}, 0)";
    private const string kInstallTableMetricSentSQL = "UPDATE install_tracking SET metric_sent=1";

#if UNITY_IPHONE
    private const string DLL_IMPORT_TARGET = "__Internal";
#else
    private const string DLL_IMPORT_TARGET = "sqlite3";
#endif

    // Sub-set of SQLite3 functions
    [DllImport(DLL_IMPORT_TARGET)]
    private extern static int sqlite3_open(string filename, out IntPtr ppDb);

    [DllImport(DLL_IMPORT_TARGET)]
    private extern static int sqlite3_close(IntPtr db);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern string sqlite3_errmsg(IntPtr db);

    [DllImport(DLL_IMPORT_TARGET)]
    private extern static int sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);

    [DllImport(DLL_IMPORT_TARGET)]
    private extern static int sqlite3_exec(IntPtr db, string zSql, IntPtr callback, IntPtr cbArg, IntPtr errmsg);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern int sqlite3_finalize(IntPtr stmHandle);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern int sqlite3_step(IntPtr stmHandle);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern long sqlite3_last_insert_rowid(IntPtr db);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern long sqlite3_column_int64(IntPtr stmHandle, int iCol);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern string sqlite3_column_text(IntPtr stmHandle, int iCol);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);

    [DllImport(DLL_IMPORT_TARGET)]
    private static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

    internal IntPtr mDBPtr;
#else // CACHE_ENABLED
    internal List<CachedRequest> mCachedRequests;
#endif
#endregion
    internal bool mIsDisposed = false;
}
// @endcond
