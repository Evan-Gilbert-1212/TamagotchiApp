using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace TamagotchiApp
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var isRunning = true;

      while (isRunning)
      {
        Console.Clear();
        Console.WriteLine("+------------------------------------+");
        Console.WriteLine("| Welcome To The Tamagotchi API App! |");
        Console.WriteLine("+------------------------------------+");
        Console.WriteLine();

        Console.WriteLine("What would you like to do today? Please enter the number of the task.");
        Console.WriteLine("1: View All Alive Pets");
        Console.WriteLine("2: Add A New Pet");
        Console.WriteLine("3: Play With A Pet");
        Console.WriteLine("4: Feed A Pet");
        Console.WriteLine("5: Scold A Pet");
        Console.WriteLine("6: Delete A Pet");
        Console.WriteLine("7: Quit The Program");

        var userInput = Int32.Parse(Console.ReadLine());

        while (userInput < 1 && userInput > 7)
        {
          Console.WriteLine("Please enter a valid selection.");

          userInput = Int32.Parse(Console.ReadLine());
        }

        switch (userInput)
        {
          case 1:
            await DisplayAlivePets(true);
            break;
          case 2:
            await AddAPet();
            break;
          case 3:
            await PlayWithPet();
            break;
          case 4:
            await FeedAPet();
            break;
          case 5:
            await ScoldAPet();
            break;
          case 6:
            await DeleteAPet();
            break;
          case 7:
            isRunning = false;
            break;
        }
      }

      Console.WriteLine("+---------------------------------+");
      Console.WriteLine("| Thank you for playing! Goodbye! |");
      Console.WriteLine("+---------------------------------+");
    }

    static async Task DisplayAlivePets(bool pause)
    {
      Console.WriteLine("Getting list of alive pets...");

      var client = new HttpClient();

      var response = await client.GetAsync("https://tamagotchi-api.herokuapp.com/api/pet/alive");

      var petList = JsonSerializer.Deserialize<List<Pet>>(await response.Content.ReadAsStringAsync());

      Console.WriteLine("+--------------------------------------------------------------------------------+");
      Console.WriteLine("| Pet ID | Pet Name      | Happiness Level | Hunger Level | Last Played With     |");
      Console.WriteLine("+--------------------------------------------------------------------------------+");

      foreach (var pet in petList)
      {
        var formatPetID = String.Format("{0,-6}", pet.ID);
        var formatPetName = String.Format("{0,-13}", pet.Name);
        var formatHappiness = String.Format("{0,-15}", pet.HappinessLevel);
        var formatHunger = String.Format("{0,-12}", pet.HungerLevel);
        var formatLastPlayedWith = String.Format("{0,-20}", pet.LastInteractedWithDate);

        Console.WriteLine($"| {formatPetID} | {formatPetName} | {formatHappiness} | {formatHunger} | {formatLastPlayedWith} |");
      }

      Console.WriteLine("+--------------------------------------------------------------------------------+");

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      if (pause)
      {
        Console.WriteLine("Press (ENTER) to return to the main menu.");
        Console.ReadLine();
      }
    }

    static async Task PlayWithPet()
    {
      await DisplayAlivePets(false);

      Console.WriteLine("What pet would you like to play with? Enter the Pet ID.");

      var petID = Int32.Parse(Console.ReadLine());

      var client = new HttpClient();

      var patchInput = new StringContent("");

      var response = await client.PatchAsync($"https://tamagotchi-api.herokuapp.com/api/pet/play/{petID}", patchInput);

      var petPlayedWith = JsonSerializer.Deserialize<Pet>(await response.Content.ReadAsStringAsync());

      Console.WriteLine($"You played with {petPlayedWith.Name}!");
      Console.WriteLine($"New Happiness Level: {petPlayedWith.HappinessLevel}");
      Console.WriteLine($"New Hunger Level: {petPlayedWith.HungerLevel}");
      Console.WriteLine();

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      Console.WriteLine("Press (ENTER) to return to the main menu.");
      Console.ReadLine();
    }

    static async Task FeedAPet()
    {
      await DisplayAlivePets(false);

      Console.WriteLine("What pet would you like to feed? Enter the Pet ID.");

      var petID = Int32.Parse(Console.ReadLine());

      var client = new HttpClient();

      var patchInput = new StringContent("");

      var response = await client.PatchAsync($"https://tamagotchi-api.herokuapp.com/api/pet/feed/{petID}", patchInput);

      var petFed = JsonSerializer.Deserialize<Pet>(await response.Content.ReadAsStringAsync());

      Console.WriteLine($"You fed {petFed.Name}!");
      Console.WriteLine($"New Happiness Level: {petFed.HappinessLevel}");
      Console.WriteLine($"New Hunger Level: {petFed.HungerLevel}");
      Console.WriteLine();

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      Console.WriteLine("Press (ENTER) to return to the main menu.");
      Console.ReadLine();
    }

    static async Task ScoldAPet()
    {
      await DisplayAlivePets(false);

      Console.WriteLine("What pet would you like to scold? Enter the Pet ID.");

      var petID = Int32.Parse(Console.ReadLine());

      var client = new HttpClient();

      var patchInput = new StringContent("");

      var response = await client.PatchAsync($"https://tamagotchi-api.herokuapp.com/api/pet/scold/{petID}", patchInput);

      var petScolded = JsonSerializer.Deserialize<Pet>(await response.Content.ReadAsStringAsync());

      Console.WriteLine($"You scolded {petScolded.Name}!");
      Console.WriteLine($"New Happiness Level: {petScolded.HappinessLevel}");
      Console.WriteLine();

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      Console.WriteLine("Press (ENTER) to return to the main menu.");
      Console.ReadLine();
    }

    static async Task AddAPet()
    {
      Console.WriteLine("What is the name of the pet you want to add?");

      var petName = Console.ReadLine();

      var client = new HttpClient();

      var petToAdd = new Pet() { Name = petName };

      var jsonPet = await Task.Run(() => JsonSerializer.Serialize<Pet>(petToAdd));

      var postInput = new StringContent(jsonPet, Encoding.UTF8, "application/json");

      var response = await client.PostAsync($"https://tamagotchi-api.herokuapp.com/api/pet/", postInput);

      var petCreated = JsonSerializer.Deserialize<Pet>(await response.Content.ReadAsStringAsync());

      Console.WriteLine($"You added {petCreated.Name}!");
      Console.WriteLine($"Birthday: {petCreated.Birthday}");
      Console.WriteLine();

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      Console.WriteLine("Press (ENTER) to return to the main menu.");
      Console.ReadLine();
    }

    static async Task DeleteAPet()
    {
      await DisplayAlivePets(false);

      Console.WriteLine("What pet would you like to delete? Enter the Pet ID.");

      var petID = Int32.Parse(Console.ReadLine());

      var client = new HttpClient();

      var response = await client.DeleteAsync($"https://tamagotchi-api.herokuapp.com/api/pet/{petID}");

      Console.WriteLine($"Pet removed successfully!");
      Console.WriteLine();

      Console.WriteLine($"Status Code: {response.StatusCode}");
      Console.WriteLine();

      Console.WriteLine("Press (ENTER) to return to the main menu.");
      Console.ReadLine();
    }
  }
}