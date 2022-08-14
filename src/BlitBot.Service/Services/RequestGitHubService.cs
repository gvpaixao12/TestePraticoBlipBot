using BlitBot.Service.Interfaces;
using BlitBot.Service.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlitBot.Service.Services
{
    public class RequestGitHubService : BaseService, IRequestGitHubService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public RequestGitHubService(IHttpClientFactory clientFactory, 
                                    IConfiguration configuration,
                                    INotificador notificador) : base(notificador)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<IEnumerable<GitHubRepo>> ObterRepositoriosDoGitHub()
        {
            var listaRepositoriosGit = new List<GitHubRepo>();
            
            listaRepositoriosGit.AddRange(FazerRequisicao(1).Result);
            
            return listaRepositoriosGit
                            .OrderBy(r => r.created_at)
                            .Where( r => (!string.IsNullOrEmpty(r.language) 
                                            && r.language.ToUpper() == "C#")).Take(5).ToList();
        }

        private async Task<IEnumerable<GitHubRepo>> FazerRequisicao(int page, int qtdPorPagina = 100)
        {
            var objetosDesserializados = new List<GitHubRepo>();

            using (var client = _clientFactory.CreateClient())
            {
                var url = _configuration.GetSection("GitHubRepos")["Takenet"] + $"repos?sort=created&per_page={qtdPorPagina}&page={page}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var conteudoReposta = await response.Content.ReadAsStringAsync();
                    objetosDesserializados.AddRange(JsonConvert.DeserializeObject<List<GitHubRepo>>(conteudoReposta));

                    if (objetosDesserializados.Count == qtdPorPagina)
                        objetosDesserializados.AddRange(FazerRequisicao(++page).Result);
                }
                else
                {
                    Notificar("Houve um problema com a conexão com repositório do Github");
                }
            }

            return objetosDesserializados;
        }
    }
}
