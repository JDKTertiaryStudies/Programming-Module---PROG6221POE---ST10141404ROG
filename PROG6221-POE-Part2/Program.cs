using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApplication
{
    public delegate void RecipeCaloriesExceededHandler(string recipeName, int totalCalories);

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    public class Step
    {
        public string Description { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }

        public event RecipeCaloriesExceededHandler RecipeCaloriesExceeded;

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }

        public void AddIngredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient
            {
                Name = name,
                Quantity = quantity,
                Unit = unit,
                Calories = calories,
                FoodGroup = foodGroup
            });
        }

        public void AddStep(string description)
        {
            Steps.Add(new Step { Description = description });
        }

        public void ScaleRecipe(double factor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        public void DisplayRecipe()
        {
            Console.WriteLine($"Recipe: {Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name}");
            }
            Console.WriteLine("Steps:");
            for (int i = 0; i < Steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Steps[i].Description}");
            }
            Console.WriteLine();
        }

        public int CalculateTotalCalories()
        {
            int totalCalories = Ingredients.Sum(i => i.Calories);
            if (totalCalories > 300)
            {
                RecipeCaloriesExceeded?.Invoke(Name, totalCalories);
            }
            return totalCalories;
        }
    }

    public class RecipeManager
    {
        private List<Recipe> recipes;

        public RecipeManager()
        {
            recipes = new List<Recipe>();
        }

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public void DisplayRecipeList()
        {
            recipes.Sort((r1, r2) => string.Compare(r1.Name, r2.Name, StringComparison.Ordinal));
            Console.WriteLine("Recipe List:");
            foreach (var recipe in recipes)
            {
                Console.WriteLine(recipe.Name);
            }
            Console.WriteLine();
        }

        public Recipe GetRecipe(string recipeName)
        {
            return recipes.FirstOrDefault(r => r.Name == recipeName);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            RecipeManager recipeManager = new RecipeManager();

            while (true)
            {
                Console.WriteLine("Enter a command:");
                Console.WriteLine("(1) Add Recipe");
                Console.WriteLine("(2) Display Recipe List");
                Console.WriteLine("(3) Select Recipe");
                Console.WriteLine("(4) Clear All Data");
                Console.WriteLine("(5) Exit");
                Console.Write("Command: ");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddRecipe(recipeManager);
                        break;
                    case "2":
                        recipeManager.DisplayRecipeList();
                        break;
                    case "3":
                        SelectRecipe(recipeManager);
                        break;
                    case "4":
                        recipeManager = new RecipeManager();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        static void AddRecipe(RecipeManager recipeManager)
        {
            Console.WriteLine("=== Add Recipe ===");
            Console.Write("Enter recipe name: ");
            string name = Console.ReadLine();

            Recipe recipe = new Recipe(name);

            Console.WriteLine("Enter the ingredients:");
            Console.Write("Number of ingredients: ");
            int ingredientCount;
            while (!int.TryParse(Console.ReadLine(), out ingredientCount))
            {
                Console.WriteLine("Invalid input. Enter a valid number.");
                Console.Write("Number of ingredients: ");
            }

            for (int i = 0; i < ingredientCount; i++)
            {
                Console.WriteLine($"Ingredient {i + 1}:");
                Console.Write("Name: ");
                string ingredientName = Console.ReadLine();
                Console.Write("Quantity: ");
                double quantity;
                while (!double.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Enter a valid number.");
                    Console.Write("Quantity: ");
                }
                Console.Write("Unit: ");
                string unit = Console.ReadLine();
                Console.Write("Calories: ");
                int calories;
                while (!int.TryParse(Console.ReadLine(), out calories))
                {
                    Console.WriteLine("Invalid input. Enter a valid number.");
                    Console.Write("Calories: ");
                }
                Console.Write("Food Group: ");
                string foodGroup = Console.ReadLine();

                recipe.AddIngredient(ingredientName, quantity, unit, calories, foodGroup);
            }

            Console.WriteLine("Enter the steps:");
            Console.Write("Number of steps: ");
            int stepCount;
            while (!int.TryParse(Console.ReadLine(), out stepCount))
            {
                Console.WriteLine("Invalid input. Enter a valid number.");
                Console.Write("Number of steps: ");
            }

            for (int i = 0; i < stepCount; i++)
            {
                Console.WriteLine($"Step {i + 1}:");
                string description = Console.ReadLine();
                recipe.AddStep(description);
            }

            recipeManager.AddRecipe(recipe);
            Console.WriteLine("Recipe added successfully.");
            Console.WriteLine();
        }

        static void SelectRecipe(RecipeManager recipeManager)
        {
            Console.WriteLine("=== Select Recipe ===");
            recipeManager.DisplayRecipeList();
            Console.Write("Enter the name of the recipe to display: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipeManager.GetRecipe(recipeName);
            if (recipe != null)
            {
                Console.WriteLine();
                recipe.DisplayRecipe();

                Console.WriteLine("Enter a command:");
                Console.WriteLine("(1) Scale Recipe");
                Console.WriteLine("(2) Reset Quantities");
                Console.WriteLine("(3) Return to Main Menu");
                Console.Write("Command: ");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        ScaleRecipe(recipe);
                        break;
                    case "2":
                        ResetQuantities(recipe);
                        break;
                    case "3":
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
            Console.WriteLine();
        }

        static void ScaleRecipe(Recipe recipe)
        {
            Console.WriteLine("=== Scale Recipe ===");
            Console.WriteLine("Enter the scaling factor: (0.5, 2, 3)");
            Console.Write("Factor: ");
            double factor;
            while (!double.TryParse(Console.ReadLine(), out factor) || (factor != 0.5 && factor != 2 && factor != 3))
            {
                Console.WriteLine("Invalid input. Enter a valid scaling factor.");
                Console.Write("Factor: ");
            }

            recipe.ScaleRecipe(factor);
            Console.WriteLine("Scaled recipe:");
            recipe.DisplayRecipe();
        }

        static void ResetQuantities(Recipe recipe)
        {
            Console.WriteLine("=== Reset Quantities ===");
            Console.WriteLine("Original recipe:");
            recipe.DisplayRecipe();
        }

        static void RecipeCaloriesExceededHandler(string recipeName, int totalCalories)
        {
            Console.WriteLine($"Warning: The total calories of recipe '{recipeName}' exceeds 300 ({totalCalories} calories).");
        }
    }
}