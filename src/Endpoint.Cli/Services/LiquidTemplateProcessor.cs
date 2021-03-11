using DotLiquid;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Endpoint.Cli.Services
{
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        public string[] Process(string[] template, IDictionary<string, object> tokens, string[] ignoreTokens = null)
        {
            Hash hash = default;

            try
            {
                if (ignoreTokens != null)
                {
                    var dictionary = ImmutableDictionary.CreateBuilder<string, object>();

                    foreach(var entry in tokens)
                    {
                        if (!ignoreTokens.Contains(entry.Key))
                        {
                            dictionary.Add(entry.Key, entry.Value);
                        }
                    }

                    hash = Hash.FromDictionary(dictionary);

                } else
                {
                    hash = Hash.FromDictionary(tokens);
                }

                var liquidTemplate = Template.Parse(string.Join(Environment.NewLine, template));

                return liquidTemplate.Render(hash).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }
            catch(Exception e)
            {
                Console.WriteLine(nameof(LiquidTemplateProcessor));

                throw e;
            }
        }

        public string Process(string template, IDictionary<string, object> tokens)
        {
            try 
            { 
                var hash = Hash.FromDictionary(tokens);

                var liquidTemplate = Template.Parse(template);

                return liquidTemplate.Render(hash);
            }
            catch (Exception e)
            {
                Console.WriteLine(nameof(LiquidTemplateProcessor));

                throw e;
            }
        }
    }



}
