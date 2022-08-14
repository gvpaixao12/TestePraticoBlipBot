using BlitBot.Service.Interfaces;
using BlitBot.Service.Notificacoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlitBot.Service.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

    }
}
