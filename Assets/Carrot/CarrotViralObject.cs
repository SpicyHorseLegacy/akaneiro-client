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
using UnityEngine;
using System.Collections.Generic;
#endregion

public partial class Carrot
{
    /// <summary>
    /// Describes a viral object which is to be created.
    /// </summary>
    public class ViralObject
    {
        /// <summary>
        /// The title of the viral object to be created.
        /// </summary>
        public string Title
        {
            get
            {
                return mObjectProperties["title"] as string;
            }
            set
            {
                mObjectProperties["title"] = value;
            }
        }

        /// <summary>
        /// The description of the viral object to be created.
        /// </summary>
        public string Description
        {
            get
            {
                return mObjectProperties["description"] as string;
            }
            set
            {
                mObjectProperties["description"] = value;
            }
        }

        /// <summary>
        /// The identifier of the viral object to create or re-use.
        /// If Identifier is not specified, GUID will be generated instead.
        /// </summary>
        public string Identifier
        {
            get
            {
                return mObjectProperties["identifier"] as string;
            }
            set
            {
                mObjectProperties["identifier"] = value;
            }
        }

        /// <summary>
        /// The URL of the image, or a Texture2D which will be uploaded and
        /// used for the viral object.
        /// </summary>
        public object Image
        {
            get
            {
                return mObjectProperties["image"];
            }
            set
            {
                mObjectProperties["image"] = value;
            }
        }

        /// <summary>
        /// Assignment of user defined fields for the viral object to created.
        /// </summary>
        public Dictionary<string, object> Fields
        {
            get
            {
                return mObjectProperties["fields"] as Dictionary<string, object>;
            }
            set
            {
                mObjectProperties["fields"] = value;
            }
        }

        /// <summary>
        /// Specify the parameters for a viral object using a Texture2D to upload.
        /// </summary>
        /// <param name="objectTypeId">Carrot object type id.</param>
        /// <param name="title">Title of the new viral object.</param>
        /// <param name="description">Description for the new viral object.</param>
        /// <param name="image">Texture2D to upload for the new viral object.</param>
        /// <param name="identifier">Optional object identifierto create, or re-use.</params>
        public ViralObject(string objectTypeId, string title, string description,
                           Texture2D image, string identifier = null)
        {
            if(string.IsNullOrEmpty(objectTypeId))
            {
                throw new ArgumentNullException("objectTypeId must not be null or empty string.", "objectTypeId");
            }

            if(string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title must not be null or empty string.", "title");
            }

            if(string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("description must not be null or empty string.", "description");
            }

            if(image == null)
            {
                throw new ArgumentNullException("image must not be null.", "image");
            }

            mObjectProperties = new Dictionary<string, object>();
            mObjectProperties["object_type"] = objectTypeId;
            this.Title = title;
            this.Description = description;
            this.Image = image;
            if(identifier != null) this.Identifier = identifier;
        }

        /// <summary>
        /// Specify the parameters for a viral object with a remote image URL.
        /// </summary>
        /// <param name="objectTypeId">Carrot object type id.</param>
        /// <param name="title">Title of the new viral object.</param>
        /// <param name="description">Description for the new viral object.</param>
        /// <param name="imageUrl">Image URL for the new viral object.</param>
        /// <param name="identifier">Optional object identifierto create, or re-use.</params>
        public ViralObject(string objectTypeId, string title, string description,
                           string imageUrl, string identifier = null)
        {
            if(string.IsNullOrEmpty(objectTypeId))
            {
                throw new ArgumentNullException("objectTypeId must not be null or empty string.", "objectTypeId");
            }

            if(string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title must not be null or empty string.", "title");
            }

            if(string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("description must not be null or empty string.", "description");
            }

            if(string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentNullException("imageUrl must not be null or empty string.", "imageUrl");
            }

            mObjectProperties = new Dictionary<string, object>();
            mObjectProperties["object_type"] = objectTypeId;
            this.Title = title;
            this.Description = description;
            this.Image = imageUrl;
            if(identifier != null) this.Identifier = identifier;
        }

        public Dictionary<string, object> toDictionary()
        {
            return mObjectProperties;
        }

        private Dictionary<string, object> mObjectProperties;
    }
}
