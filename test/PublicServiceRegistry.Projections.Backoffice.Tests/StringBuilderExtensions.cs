namespace PublicServiceRegistry.Projections.Backoffice.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendTitleBlock(
            this StringBuilder builder,
            string title,
            Action<StringBuilder> addContent)
        {
            builder.AppendLine($"{title}:");
            addContent(builder);
            builder.AppendLine();

            return builder;
        }

        public static StringBuilder AppendTitleBlock(
            this StringBuilder builder,
            string title,
            string content)
        {
            return builder.AppendTitleBlock(title, b => b.AppendLine(content));
        }

        public static StringBuilder AppendTitleBlock<T>(
            this StringBuilder builder,
            string title,
            IEnumerable<T> collection,
            Func<T, string> formatter)
        {
            return builder.AppendTitleBlock(title, b => b.AppendLines(collection, formatter));
        }

        public static StringBuilder AppendLines<T>(
            this StringBuilder builder,
            IEnumerable<T> collection,
            Func<T, string> formatter)
        {
            foreach (var item in collection)
                builder.AppendLine(formatter(item));

            return builder;
        }
    }
}