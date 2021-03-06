// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TraceCollector
{
    using System;
    using TestPlatform.ObjectModel.DataCollection;

    /// <summary>
    /// Wrapper for <see cref="Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection.DataCollectionLogger"/>
    /// to make the collector testable.
    /// </summary>
    internal interface IDataCollectionLogger
    {
        void LogError(DataCollectionContext context, Exception exception);

        void LogError(DataCollectionContext context, string text);

        void LogError(DataCollectionContext context, string text, Exception exception);

        void LogWarning(DataCollectionContext context, string text);
    }
}