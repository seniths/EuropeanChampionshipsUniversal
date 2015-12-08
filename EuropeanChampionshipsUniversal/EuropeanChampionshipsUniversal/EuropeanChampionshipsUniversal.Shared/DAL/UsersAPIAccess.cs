﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EuropeanChampionshipsUniversal.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EuropeanChampionshipsUniversal.DAL
{
    public class UsersAPIAccess : IUsersDataAccess
    {
        private HttpClient client;

        public UsersAPIAccess()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://europeanchampionships.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> AddFavoriteTeam(int idUser, int idTeam)
        {
            IChampionshipsDataAccess daTeams = new ChampionshipsAPIAccess();
            int idChampionship = await daTeams.GetChampionshipIdByTeamId(idTeam);
            FavoriteTeamsUser fav = new FavoriteTeamsUser() { idTeam = idTeam, idUser = idUser, idChampionship = idChampionship};

            var json = JsonConvert.SerializeObject(fav);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/favoriteTeamsUsers", content);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<User> GetUserById(int idUser)
        {
            HttpResponseMessage response = await client.GetAsync("api/users/" + idUser);
            var user = new User();
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(json);
            }
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/users");
            var users = new List<User>();
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(json);
            }
            return users;
        }

        public async Task<bool> RemoveFavoriteTeam(int idToDelete)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Favoriteteamsusers/" + idToDelete);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> SaveUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/users", content);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
