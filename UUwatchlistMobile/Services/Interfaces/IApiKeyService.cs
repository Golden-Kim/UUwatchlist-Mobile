using System;
using System.Collections.Generic;
using System.Text;

namespace UUwatchlistMobile.Services.Interfaces
{
    internal interface IApiKeyService
    {
        /// <summary>
        /// Restituisce true se la chiave è presente e non è vuota
        /// </summary>
        public bool HasKey { get; }

        /// <summary>
        /// Metodo per recuperare la chiave dalle preferenze native
        /// </summary>
        /// <returns></returns>
        public string GetKey();

        /// <summary>
        /// Metodo per salvare la chiave
        /// </summary>
        /// <param name="key"></param>
        public void SaveKey(string key);

        /// <summary>
        /// Metodo per rimuovere la chiave (es. se l'utente fa un logout o reset)
        /// </summary>
        public void DeleteKey();


    }
}
