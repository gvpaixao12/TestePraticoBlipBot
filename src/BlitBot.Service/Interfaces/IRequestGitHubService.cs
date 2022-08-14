using BlitBot.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlitBot.Service.Interfaces
{
    public interface IRequestGitHubService
    {
        Task<IEnumerable<GitHubRepo>> ObterRepositoriosDoGitHub();
    }
}
