/* ========================================================================
 * Copyright (c) 2005-2016 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace OPCModel
{
    #region Object Identifiers
    /// <summary>
    /// A class that declares constants for all Objects in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <summary>
        /// The identifier for the Book_Author Object.
        /// </summary>
        public const uint Book_Author = 52;
    }
    #endregion

    #region ObjectType Identifiers
    /// <summary>
    /// A class that declares constants for all ObjectTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypes
    {
        /// <summary>
        /// The identifier for the Book ObjectType.
        /// </summary>
        public const uint Book = 1;

        /// <summary>
        /// The identifier for the Author ObjectType.
        /// </summary>
        public const uint Author = 4;
    }
    #endregion

    #region Variable Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public const uint Book_Id = 20;

        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public const uint Book_Title = 2;

        /// <summary>
        /// The identifier for the Author_Id Variable.
        /// </summary>
        public const uint Author_Id = 21;

        /// <summary>
        /// The identifier for the Author_FirstName Variable.
        /// </summary>
        public const uint Author_FirstName = 22;

        /// <summary>
        /// The identifier for the Author_LastName Variable.
        /// </summary>
        public const uint Author_LastName = 23;

        /// <summary>
        /// The identifier for the Author_NickName Variable.
        /// </summary>
        public const uint Author_NickName = 24;
    }
    #endregion

    #region Object Node Identifiers
    /// <summary>
    /// A class that declares constants for all Objects in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <summary>
        /// The identifier for the Book_Author Object.
        /// </summary>
        public static readonly ExpandedNodeId Book_Author = new ExpandedNodeId(OPCModel.Objects.Book_Author, OPCModel.Namespaces.OPCModel);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <summary>
    /// A class that declares constants for all ObjectTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <summary>
        /// The identifier for the Book ObjectType.
        /// </summary>
        public static readonly ExpandedNodeId Book = new ExpandedNodeId(OPCModel.ObjectTypes.Book, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Author ObjectType.
        /// </summary>
        public static readonly ExpandedNodeId Author = new ExpandedNodeId(OPCModel.ObjectTypes.Author, OPCModel.Namespaces.OPCModel);
    }
    #endregion

    #region Variable Node Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Id = new ExpandedNodeId(OPCModel.Variables.Book_Id, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Title = new ExpandedNodeId(OPCModel.Variables.Book_Title, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Author_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_Id = new ExpandedNodeId(OPCModel.Variables.Author_Id, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Author_FirstName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_FirstName = new ExpandedNodeId(OPCModel.Variables.Author_FirstName, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Author_LastName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_LastName = new ExpandedNodeId(OPCModel.Variables.Author_LastName, OPCModel.Namespaces.OPCModel);

        /// <summary>
        /// The identifier for the Author_NickName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_NickName = new ExpandedNodeId(OPCModel.Variables.Author_NickName, OPCModel.Namespaces.OPCModel);
    }
    #endregion

    #region BrowseName Declarations
    /// <summary>
    /// Declares all of the BrowseNames used in the Model Design.
    /// </summary>
    public static partial class BrowseNames
    {
        /// <summary>
        /// The BrowseName for the Author component.
        /// </summary>
        public const string Author = "Author";

        /// <summary>
        /// The BrowseName for the Book component.
        /// </summary>
        public const string Book = "Book";

        /// <summary>
        /// The BrowseName for the FirstName component.
        /// </summary>
        public const string FirstName = "FirstName";

        /// <summary>
        /// The BrowseName for the Id component.
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// The BrowseName for the LastName component.
        /// </summary>
        public const string LastName = "LastName";

        /// <summary>
        /// The BrowseName for the NickName component.
        /// </summary>
        public const string NickName = "NickName";

        /// <summary>
        /// The BrowseName for the Title component.
        /// </summary>
        public const string Title = "Title";
    }
    #endregion

    #region Namespace Declarations
    /// <summary>
    /// Defines constants for all namespaces referenced by the model design.
    /// </summary>
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the OPCModel namespace (.NET code namespace is 'OPCModel').
        /// </summary>
        public const string OPCModel = "http://tpum.example.com";
    }
    #endregion

    #region BookState Class
    #if (!OPCUA_EXCLUDE_BookState)
    /// <summary>
    /// Stores an instance of the Book ObjectType.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BookState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes the type with its default attribute values.
        /// </summary>
        public BookState(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// Returns the id of the default type definition node for the instance.
        /// </summary>
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(OPCModel.ObjectTypes.Book, OPCModel.Namespaces.OPCModel, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected override void Initialize(ISystemContext context)
        {
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <summary>
        /// Initializes the any option children defined for the instance.
        /// </summary>
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (Title != null)
            {
                Title.Initialize(context, Title_InitializationString);
            }

            if (Author != null)
            {
                Author.Initialize(context, Author_InitializationString);
            }
        }

        #region Initialization String
        private const string Title_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8VYIkKAgAAAAEABQAAAFRpdGxlAQECAAAu" +
           "AEQCAAAAAAz/////AwP/////AAAAAA==";

        private const string Author_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8EYIAKAQAAAAEABgAAAEF1dGhvcgEBNAAA" +
           "LwEBBAA0AAAA/////wAAAAA=";

        private const string InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8EYIAAAQAAAAEADAAAAEJvb2tJbnN0YW5j" +
           "ZQEBAQABAQEA/////wMAAAAVYIkKAgAAAAEAAgAAAElkAQEUAAAuAEQUAAAAAAb/////AwP/////AAAA" +
           "ABVgiQoCAAAAAQAFAAAAVGl0bGUBAQIAAC4ARAIAAAAADP////8DA/////8AAAAABGCACgEAAAABAAYA" +
           "AABBdXRob3IBATQAAC8BAQQANAAAAP////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <summary>
        /// A description for the Id Property.
        /// </summary>
        public PropertyState<int> Id
        {
            get
            {
                return m_id;
            }

            set
            {
                if (!Object.ReferenceEquals(m_id, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_id = value;
            }
        }

        /// <summary>
        /// A description for the Title Property.
        /// </summary>
        public PropertyState<string> Title
        {
            get
            {
                return m_title;
            }

            set
            {
                if (!Object.ReferenceEquals(m_title, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_title = value;
            }
        }

        /// <summary>
        /// A description for the Author Object.
        /// </summary>
        public AuthorState Author
        {
            get
            {
                return m_author;
            }

            set
            {
                if (!Object.ReferenceEquals(m_author, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_author = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Populates a list with the children that belong to the node.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="children">The list of children to populate.</param>
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_id != null)
            {
                children.Add(m_id);
            }

            if (m_title != null)
            {
                children.Add(m_title);
            }

            if (m_author != null)
            {
                children.Add(m_author);
            }

            base.GetChildren(context, children);
        }

        /// <summary>
        /// Finds the child with the specified browse name.
        /// </summary>
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case OPCModel.BrowseNames.Id:
                {
                    if (createOrReplace)
                    {
                        if (Id == null)
                        {
                            if (replacement == null)
                            {
                                Id = new PropertyState<int>(this);
                            }
                            else
                            {
                                Id = (PropertyState<int>)replacement;
                            }
                        }
                    }

                    instance = Id;
                    break;
                }

                case OPCModel.BrowseNames.Title:
                {
                    if (createOrReplace)
                    {
                        if (Title == null)
                        {
                            if (replacement == null)
                            {
                                Title = new PropertyState<string>(this);
                            }
                            else
                            {
                                Title = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Title;
                    break;
                }

                case OPCModel.BrowseNames.Author:
                {
                    if (createOrReplace)
                    {
                        if (Author == null)
                        {
                            if (replacement == null)
                            {
                                Author = new AuthorState(this);
                            }
                            else
                            {
                                Author = (AuthorState)replacement;
                            }
                        }
                    }

                    instance = Author;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<int> m_id;
        private PropertyState<string> m_title;
        private AuthorState m_author;
        #endregion
    }
    #endif
    #endregion

    #region AuthorState Class
    #if (!OPCUA_EXCLUDE_AuthorState)
    /// <summary>
    /// Stores an instance of the Author ObjectType.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class AuthorState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes the type with its default attribute values.
        /// </summary>
        public AuthorState(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// Returns the id of the default type definition node for the instance.
        /// </summary>
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(OPCModel.ObjectTypes.Author, OPCModel.Namespaces.OPCModel, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected override void Initialize(ISystemContext context)
        {
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <summary>
        /// Initializes the any option children defined for the instance.
        /// </summary>
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (Id != null)
            {
                Id.Initialize(context, Id_InitializationString);
            }

            if (FirstName != null)
            {
                FirstName.Initialize(context, FirstName_InitializationString);
            }

            if (LastName != null)
            {
                LastName.Initialize(context, LastName_InitializationString);
            }

            if (NickName != null)
            {
                NickName.Initialize(context, NickName_InitializationString);
            }
        }

        #region Initialization String
        private const string Id_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8VYIkKAgAAAAEAAgAAAElkAQEVAAAuAEQV" +
           "AAAAAAb/////AQH/////AAAAAA==";

        private const string FirstName_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8VYIkKAgAAAAEACQAAAEZpcnN0TmFtZQEB" +
           "FgAALgBEFgAAAAAM/////wMD/////wAAAAA=";

        private const string LastName_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8VYIkKAgAAAAEACAAAAExhc3ROYW1lAQEX" +
           "AAAuAEQXAAAAAAz/////AwP/////AAAAAA==";

        private const string NickName_InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8VYIkKAgAAAAEACAAAAE5pY2tOYW1lAQEY" +
           "AAAuAEQYAAAAAAz/////AwP/////AAAAAA==";

        private const string InitializationString =
           "AQAAABcAAABodHRwOi8vdHB1bS5leGFtcGxlLmNvbf////8EYIAAAQAAAAEADgAAAEF1dGhvckluc3Rh" +
           "bmNlAQEEAAEBBAD/////BAAAABVgiQoCAAAAAQACAAAASWQBARUAAC4ARBUAAAAABv////8BAf////8A" +
           "AAAAFWCJCgIAAAABAAkAAABGaXJzdE5hbWUBARYAAC4ARBYAAAAADP////8DA/////8AAAAAFWCJCgIA" +
           "AAABAAgAAABMYXN0TmFtZQEBFwAALgBEFwAAAAAM/////wMD/////wAAAAAVYIkKAgAAAAEACAAAAE5p" +
           "Y2tOYW1lAQEYAAAuAEQYAAAAAAz/////AwP/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <summary>
        /// A description for the Id Property.
        /// </summary>
        public PropertyState<int> Id
        {
            get
            {
                return m_id;
            }

            set
            {
                if (!Object.ReferenceEquals(m_id, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_id = value;
            }
        }

        /// <summary>
        /// A description for the FirstName Property.
        /// </summary>
        public PropertyState<string> FirstName
        {
            get
            {
                return m_firstName;
            }

            set
            {
                if (!Object.ReferenceEquals(m_firstName, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_firstName = value;
            }
        }

        /// <summary>
        /// A description for the LastName Property.
        /// </summary>
        public PropertyState<string> LastName
        {
            get
            {
                return m_lastName;
            }

            set
            {
                if (!Object.ReferenceEquals(m_lastName, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_lastName = value;
            }
        }

        /// <summary>
        /// A description for the NickName Property.
        /// </summary>
        public PropertyState<string> NickName
        {
            get
            {
                return m_nickName;
            }

            set
            {
                if (!Object.ReferenceEquals(m_nickName, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_nickName = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Populates a list with the children that belong to the node.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="children">The list of children to populate.</param>
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_id != null)
            {
                children.Add(m_id);
            }

            if (m_firstName != null)
            {
                children.Add(m_firstName);
            }

            if (m_lastName != null)
            {
                children.Add(m_lastName);
            }

            if (m_nickName != null)
            {
                children.Add(m_nickName);
            }

            base.GetChildren(context, children);
        }

        /// <summary>
        /// Finds the child with the specified browse name.
        /// </summary>
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case OPCModel.BrowseNames.Id:
                {
                    if (createOrReplace)
                    {
                        if (Id == null)
                        {
                            if (replacement == null)
                            {
                                Id = new PropertyState<int>(this);
                            }
                            else
                            {
                                Id = (PropertyState<int>)replacement;
                            }
                        }
                    }

                    instance = Id;
                    break;
                }

                case OPCModel.BrowseNames.FirstName:
                {
                    if (createOrReplace)
                    {
                        if (FirstName == null)
                        {
                            if (replacement == null)
                            {
                                FirstName = new PropertyState<string>(this);
                            }
                            else
                            {
                                FirstName = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = FirstName;
                    break;
                }

                case OPCModel.BrowseNames.LastName:
                {
                    if (createOrReplace)
                    {
                        if (LastName == null)
                        {
                            if (replacement == null)
                            {
                                LastName = new PropertyState<string>(this);
                            }
                            else
                            {
                                LastName = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = LastName;
                    break;
                }

                case OPCModel.BrowseNames.NickName:
                {
                    if (createOrReplace)
                    {
                        if (NickName == null)
                        {
                            if (replacement == null)
                            {
                                NickName = new PropertyState<string>(this);
                            }
                            else
                            {
                                NickName = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = NickName;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<int> m_id;
        private PropertyState<string> m_firstName;
        private PropertyState<string> m_lastName;
        private PropertyState<string> m_nickName;
        #endregion
    }
    #endif
    #endregion
}