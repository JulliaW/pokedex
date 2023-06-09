﻿using Microsoft.AspNetCore.Mvc;
using pokedex.Models;
using Pokedex.Models;
using System.Text.Json;
using static pokedex.Models.PokemonList;

namespace pokedex.Controllers
{
    public class PokemonController : Controller
    {
        public async Task<IActionResult> Index(int? pagina = 0)
        {
            const string apiUrl = "http://pokeapi.co/api/v2/pokemon";
            int limit = 200;
            var httpClient = new HttpClient();
            var resonse = await httpClient.GetAsync($"{apiUrl}?limit={limit}&offset={limit * pagina}");

            var content = await resonse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PokemonApiResponse>(content);

            int? i = limit * pagina;

            var pokemonsList = new PokemonList();
            foreach(var p in result.Results)
            {
                i++;

                Pokemon pokemon = new();
                pokemon.Name = p.Name;
                pokemon.Url = $"https://assets.pokemon.com/assets/cms2/img/pokedex/detail/{((i < 1000) ? i?.ToString("#000") : i)}.png";
                
                pokemonsList.Pokemons.Add(pokemon);
                pokemonsList.Page = pagina;

            }

            return View(pokemonsList);
        }

        public async Task<IActionResult> Details(int id)
        {
            const string apiUrl = "http://pokeapi.co/api/v2/pokemon";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{apiUrl}/{id}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PokemonDetails.Root>(content);

            result.img = $"https://assets.pokemon.com/assets/cms2/img/pokedex/detail/{id.ToString("#000")}.png";

            return PartialView(result);

        }
    }
}
