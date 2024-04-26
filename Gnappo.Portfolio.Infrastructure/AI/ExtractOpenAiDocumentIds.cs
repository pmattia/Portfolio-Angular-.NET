using Azure.AI.OpenAI.Assistants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Gnappo.Portfolio.Infrastructure.AI
{
    public class ExtractOpenAiDocumentIds
    {
        //private readonly string _documentIdToken = "file-";
        public Dictionary<string,string> ExtractDocumentIds(MessageTextContent message)
        {
            //var regex = new Regex($@"\[{_documentIdToken}(\w+)\]");
            //var matches = regex.Matches(message);
            //var documentIds = new List<string>();

            //foreach (Match match in matches)
            //{
            //    documentIds.Add(_documentIdToken + match.Groups[1].Value);
            //}

            var documentIds = new Dictionary<string,string>();

            foreach (var annotation in message.Annotations)
            {
                if (annotation is MessageTextFileCitationAnnotation fileAnnotation)
                {
                    documentIds.Add(fileAnnotation.Text, fileAnnotation.FileId);
                }   
            }

            return documentIds;
        }
    }
}
