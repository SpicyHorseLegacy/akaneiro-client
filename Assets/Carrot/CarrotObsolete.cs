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

#region References
using System;
using CarrotInc.MiniJSON;
using System.Net;
using UnityEngine;
using System.Security;
using System.Collections;
using System.Net.Security;
using CarrotInc.Amazon.Util;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
#endregion

public partial class Carrot
{
    /// <summary>
    /// Sends an Open Graph action which will create a new object from the properties provided.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="objectId">Carrot object id.</param>
    /// <param name="objectProperties">Parameters to be submitted with the action.</param>
    /// <param name="objectInstanceId">Object instance id to create or re-use.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    [Obsolete("Please use the postAction() methods which take a CarrotViralObject instead.")]
    public void postAction(string actionId, string objectId, IDictionary objectProperties,
                           string objectInstanceId = null, CarrotRequestResponse callback = null)
    {
        postAction(actionId, null, objectId, objectProperties, objectInstanceId, callback);
    }

    /// <summary>
    /// Sends an Open Graph action which will create a new object from the properties provided.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="actionProperties">Parameters to be submitted with the action.</param>
    /// <param name="objectId">Carrot object id.</param>
    /// <param name="objectProperties">Parameters to be submitted with the action.</param>
    /// <param name="objectInstanceId">Object instance id to create or re-use.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    [Obsolete("Please use the postAction() methods which take a CarrotViralObject instead.")]
    public void postAction(string actionId, IDictionary actionProperties, string objectId,
                           IDictionary objectProperties, string objectInstanceId = null,
                           CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(objectId))
        {
            throw new ArgumentNullException("objectId must not be null or empty string.", "objectId");
        }

        if(string.IsNullOrEmpty(actionId))
        {
            throw new ArgumentNullException("actionId must not be null or empty string.", "actionId");
        }

        if(objectProperties == null)
        {
            throw new ArgumentNullException("objectProperties must not be null.", "objectProperties");
        }
        else if(!objectProperties.Contains("title") ||
                !objectProperties.Contains("description") ||
                !objectProperties.Contains("image"))
        {
            throw new ArgumentException("objectProperties must contain keys for 'title', 'description', and 'image'.", "objectProperties");
        }

        objectProperties["object_type"] = objectId;
        if(!string.IsNullOrEmpty(objectInstanceId)) objectProperties["object_instance_id"] = objectInstanceId;
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"action_id", actionId},
            {"action_properties", actionProperties == null ? new Dictionary<string, object>() : actionProperties},
            {"object_properties", objectProperties}
        };
        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/actions.json", parameters, callback));
    }
}
