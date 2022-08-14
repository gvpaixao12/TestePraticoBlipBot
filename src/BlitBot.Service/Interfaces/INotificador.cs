using BlitBot.Service.Notificacoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlitBot.Service.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
