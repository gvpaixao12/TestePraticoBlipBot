using BlitBot.Service.Model;
using Bogus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlipBot.API.Service
{
    [CollectionDefinition(nameof(RepositorioCollections))]
    public class RepositorioCollections : ICollectionFixture<RepositoriosTestesFixture>
    {
    }
    public class RepositoriosTestesFixture : IDisposable
    {
        public GitHubRepo GerarRepositorio()
        {
            var linguagens = new List<string> { "C#", "JavaScript", "Rust", "C", "Java", "Perl" };
            var gitRepo = new Faker<GitHubRepo>("pt_BR")
                .CustomInstantiator(f => new GitHubRepo()
                                    {
                                        name = f.Name.FirstName(),
                                        language = f.PickRandom<string>(linguagens),
                                        description = f.Lorem.Words(new Random().Next(10)).ToString(),
                                        created_at = f.Date.Between(DateTime.Now.AddYears(-3), DateTime.Now)
                                    }
                );
            return gitRepo;
        }
        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }
}
