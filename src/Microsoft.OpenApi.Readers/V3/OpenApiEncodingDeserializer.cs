﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers.ParseNodes;

namespace Microsoft.OpenApi.Readers.V3
{
    /// <summary>
    /// Class containing logic to deserialize Open API V3 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class OpenApiV3Deserializer
    {
        private static readonly FixedFieldMap<OpenApiEncoding> EncodingFixedFields = new FixedFieldMap<OpenApiEncoding>
        {
            {
                "contentType", (o, n) =>
                {
                    o.ContentType = n.GetScalarValue();
                }
            },
            {
                "headers", (o, n) =>
                {
                    o.Headers = n.CreateMap(LoadHeader);
                }
            },
            {
                "style", (o, n) =>
                {
                    ParameterStyle style;
                    if (Enum.TryParse(n.GetScalarValue(), out style))
                    {
                        o.Style = style;
                    }
                    else
                    {
                        o.Style = null;
                    }
                }
            },
            {
                "explode", (o, n) =>
                {
                    o.Explode = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "allowedReserved", (o, n) =>
                {
                    o.AllowReserved = bool.Parse(n.GetScalarValue());
                }
            },
        };

        private static readonly PatternFieldMap<OpenApiEncoding> EncodingPatternFields =
            new PatternFieldMap<OpenApiEncoding>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, n.CreateAny())}
            };

        public static OpenApiEncoding LoadEncoding(ParseNode node)
        {
            var mapNode = node.CheckMapNode("encoding");

            var encoding = new OpenApiEncoding();
            foreach (var property in mapNode)
            {
                property.ParseField(encoding, EncodingFixedFields, EncodingPatternFields);
            }

            return encoding;
        }
    }
}