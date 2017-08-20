// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace SpecificationTranslator.Query
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class OracleSqlGenerationHelper : RelationalSqlGenerationHelper
    {
        private const string OracleDateTimeFormatConst = "YYYY-MM-DD HH24:MI:SS";
        private const string DateTimeFormatConst = "yyyy-MM-dd HH:mm:ss";
        private const string DateTimeFormatStringConst = "'{0:" + DateTimeFormatConst + "}'";
        private const string DateTimeOffsetFormatConst = DateTimeFormatConst;
        private const string DateTimeOffsetFormatStringConst = DateTimeFormatStringConst;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override string DateTimeFormat => DateTimeFormatConst;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override string DateTimeFormatString => DateTimeFormatStringConst;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override string DateTimeOffsetFormat => DateTimeOffsetFormatConst;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override string DateTimeOffsetFormatString => DateTimeOffsetFormatStringConst;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string GenerateLiteralValue(DateTime value)
            => $" TO_DATE('{value.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}', '{OracleDateTimeFormatConst}')"; // Interpolation okay; strings

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override string GenerateLiteralValue(DateTimeOffset value)
            => $" TO_DATE('{value.ToString(DateTimeOffsetFormat, CultureInfo.InvariantCulture)}', '{OracleDateTimeFormatConst}')"; // Interpolation okay; strings
    }
}
