using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RecipeApplication
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public double TotalCalories { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe(string name, List<Ingredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }

        public void CalculateTotalCalories()
        {
            TotalCalories = Ingredients.Sum(x => x.Calories);
        }
    }

    public class RecipeApplication
    {
        private static List<Recipe> _recipes = new List<Recipe>();

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("1. Add recipe\n2. Display recipe list\n3. Exit");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddRecipe();
                        break;
                    case "2":
                        DisplayRecipeList();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void AddRecipe()
        {
            Console.WriteLine("Enter recipe name: ");
            var name = Console.ReadLine();

            var ingredients = new List<Ingredient>();
            Console.WriteLine("Enter number of ingredients: ");
            var ingredientCount = Convert.ToInt32(Console.ReadLine());
            for (var i = 0; i < ingredientCount; i++)
            {
                Console.WriteLine($"Enter name of ingredient {i + 1}: ");
                var ingredientName = Console.ReadLine();

                Console.WriteLine($"Enter quantity of {ingredientName}: ");
                var ingredientQuantity = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine($"Enter unit of measurement for {ingredientQuantity} {ingredientName}: ");
                var ingredientUnit = Console.ReadLine();

                Console.WriteLine($"Enter number of calories per {ingredientName}: ");
                var ingredientCalories = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine($"Enter food group for {ingredientName}: ");
                var ingredientFoodGroup = Console.ReadLine();

                ingredients.Add(new Ingredient
                {
                    Name = ingredientName,
                    Quantity = ingredientQuantity,
                    Unit = ingredientUnit,
                    Calories = ingredientCalories,
                    FoodGroup = ingredientFoodGroup
                });
            }

            var steps = new List<string>();
            Console.WriteLine("Enter number of steps: ");
            var stepCount = Convert.ToInt32(Console.ReadLine());
            for (var i = 0; i < stepCount; i++)
            {
                Console.WriteLine($"Enter step {i + 1}: ");
                var step = Console.ReadLine();
                steps.Add(step);
            }

            var recipe = new Recipe(name, ingredients, steps);
            recipe.CalculateTotalCalories();

            if (recipe.TotalCalories > 300)
            {
                Console.WriteLine("Warning: This recipe has over 300 calories!");
            }

            _recipes.Add(recipe);
            Console.WriteLine("Recipe added successfully!");
        }

        private static void DisplayRecipeList()
        {
            if (_recipes.Count == 0)
            {
                Console.WriteLine("No recipes found.");
                return;
            }

            Console.WriteLine("List of recipes:");
            var recipeNames = _recipes.Select(x => x.Name).OrderBy(x => x).ToList();
            foreach (var recipeName in recipeNames)
            {
                Console.WriteLine(recipeName);
            }

            Console.WriteLine("Enter recipe name to display: ");
            var recipeNameToDisplay = Console.ReadLine();

            var recipeToDisplay = _recipes.FirstOrDefault(x => x.Name.Equals(recipeNameToDisplay, StringComparison.OrdinalIgnoreCase));
            if (recipeToDisplay == null)
            {
                Console.WriteLine($"Recipe named {recipeNameToDisplay} not found.");
                return;
            }

            Console.WriteLine($"Name: {recipeToDisplay.Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in recipeToDisplay.Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name}, {ingredient.Calories} calories, {ingredient.FoodGroup} food group");
            }
            Console.WriteLine("Steps:");
            for (var i = 0; i < recipeToDisplay.Steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipeToDisplay.Steps[i]}");
            }
            Console.WriteLine($"Total calories: {recipeToDisplay.TotalCalories}");
        }
    }
}