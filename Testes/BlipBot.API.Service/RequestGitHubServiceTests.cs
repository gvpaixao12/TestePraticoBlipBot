using BlitBot.Service.Interfaces;
using BlitBot.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlipBot.API.Service
{
    [Collection(nameof(RepositorioCollections))]
    public class RequestGitHubServiceTests
    {
        private readonly RepositoriosTestesFixture _repositoriosTestesFixture;

        public RequestGitHubServiceTests(RepositoriosTestesFixture repositoriosTestesFixture)
        {
            _repositoriosTestesFixture = repositoriosTestesFixture;
        }

        [Fact(DisplayName = "Obter repositórios")]
        [Trait("Service", "ResquestGitHubService Testes")]
        public async Task ObterLista_Repostorios_Filtrados()
        {

            //Arrange
            var repo = _repositoriosTestesFixture.GerarRepositorio();
            var http = new Mock<IHttpClientFactory>();
            var httpCliente = new Mock<HttpClient>();
            var configBuilder = new ConfigurationBuilder();
            var notificador = new Mock<INotificador>();
            var appSett = new Dictionary<string, string>() 
                { { "GitHubRepos:Takenet", "https://api.github.com/orgs/takenet/" } };


            IConfiguration config = configBuilder.AddInMemoryCollection(appSett).Build();

            httpCliente.Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(repo.ToString()) }));

            http.Setup(h => h.CreateClient(It.IsAny<string>())).Returns(httpCliente.Object);
            

            //Act
            var gitService = new RequestGitHubService(http.Object, config, notificador.Object);
            var resultado = await gitService.ObterRepositoriosDoGitHub();

            //Assert
            Assert.Equal(5, resultado.Count());
        }
    }
}
