﻿using System;
using System.Collections.Generic;

namespace Domain.SearchResources.SearchInstanceKeys
{
    public class SearchInstanceKeyGenerationRequest
    {
        public Guid SearchInstanceId { get; set; }
        public Guid? SearchIndexId { get; set; }
        public IEnumerable<SearchInstanceKeyRequest> KeysToGenerate { get; set; }
    }
}
