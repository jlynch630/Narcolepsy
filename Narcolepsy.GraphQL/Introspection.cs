﻿namespace Narcolepsy.GraphQL {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

    internal class Introspection {
        private const string IntrospectionQuery = """
                                                    query IntrospectionQuery {
                                                      __schema {

                                                        queryType { name }
                                                        mutationType { name }
                                                        subscriptionType { name }
                                                        types {
                                                          ...FullType
                                                        }
                                                        directives {
                                                          name
                                                          description

                                                          locations
                                                          args {
                                                            ...InputValue
                                                          }
                                                        }
                                                      }
                                                    }
                                                    fragment FullType on __Type {
                                                      kind
                                                      name
                                                      description

                                                      fields(includeDeprecated: true) {
                                                        name
                                                        description
                                                        args {
                                                          ...InputValue
                                                        }
                                                        type {
                                                          ...TypeRef
                                                        }
                                                        isDeprecated
                                                        deprecationReason
                                                      }
                                                      inputFields {
                                                        ...InputValue
                                                      }
                                                      interfaces {
                                                        ...TypeRef
                                                      }
                                                      enumValues(includeDeprecated: true) {
                                                        name
                                                        description
                                                        isDeprecated
                                                        deprecationReason
                                                      }
                                                      possibleTypes {
                                                        ...TypeRef
                                                      }
                                                    }
                                                    fragment InputValue on __InputValue {
                                                      name
                                                      description
                                                      type { ...TypeRef }
                                                      defaultValue


                                                    }
                                                    fragment TypeRef on __Type {
                                                      kind
                                                      name
                                                      ofType {
                                                        kind
                                                        name
                                                        ofType {
                                                          kind
                                                          name
                                                          ofType {
                                                            kind
                                                            name
                                                            ofType {
                                                              kind
                                                              name
                                                              ofType {
                                                                kind
                                                                name
                                                                ofType {
                                                                  kind
                                                                  name
                                                                  ofType {
                                                                    kind
                                                                    name
                                                                  }
                                                                }
                                                              }
                                                            }
                                                          }
                                                        }
                                                      }
                                                    }
                                                    """;
        public async Task<string> GetSchema(string url) {
            HttpClient Client = new();
            JsonObject Body = new() {
                ["query"] = Introspection.IntrospectionQuery
            };

            HttpResponseMessage Result = await Client.PostAsync(url, new StringContent(Body.ToJsonString(), Encoding.UTF8, "application/json"));
            return await Result.Content.ReadAsStringAsync();
        }
    }
}
