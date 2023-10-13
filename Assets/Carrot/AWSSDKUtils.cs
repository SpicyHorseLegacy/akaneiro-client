#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_DASHBOARD_WIDGET || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_NACL || UNITY_FLASH
#  define UNITY
#endif

/*******************************************************************************
 *  Copyright 2008-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CarrotInc.Amazon.Util
{
    /// <summary>
    /// This class defines utilities and constants that can be used by 
    /// all the client libraries of the SDK.
    /// </summary>
    public static class AWSSDKUtils
    {
        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature
        /// </summary>
        /// <param name="data">The data to be signed</param>
        /// <param name="key">The secret signing key</param>
        /// <param name="algorithm">The algorithm to sign the data with</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        /// <returns>A string representing the HMAC signature</returns>
#if UNITY
        public static string HMACSign(string data, string key, KeyedHashAlgorithm algorithm)
#else
        public static string HMACSign(string data, System.Security.SecureString key, KeyedHashAlgorithm algorithm)
#endif
        {
#if UNITY
            if (String.IsNullOrEmpty(key))
#else
            if (null == key)
#endif
            {
                throw new ArgumentNullException("key", "The AWS Secret Access Key specified is NULL!");
            }

            if (String.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data", "Please specify data to sign.");
            }

            if (null == algorithm)
            {
                throw new ArgumentNullException("algorithm", "Please specify a KeyedHashAlgorithm to use.");
            }
#if UNITY
            try
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key);
                return Convert.ToBase64String(algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(data.ToCharArray()))
                    );
            }
            finally
            {
                algorithm.Clear();
            }
#else
            // pointer to hold unmanaged reference to SecureString instance
            IntPtr bstr = IntPtr.Zero;
            char[] charArray = new char[key.Length];
            try
            {
                // Marshal SecureString into byte array
                bstr = Marshal.SecureStringToBSTR(key);
                Marshal.Copy(bstr, charArray, 0, charArray.Length);
                algorithm.Key = Encoding.UTF8.GetBytes(charArray);
                return Convert.ToBase64String(algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(data.ToCharArray()))
                    );
            }
            finally
            {
                // Make sure that the clear text data is zeroed out
                Marshal.ZeroFreeBSTR(bstr);
                algorithm.Clear();
                Array.Clear(charArray, 0, charArray.Length);
            }
#endif
        }
    }
}
