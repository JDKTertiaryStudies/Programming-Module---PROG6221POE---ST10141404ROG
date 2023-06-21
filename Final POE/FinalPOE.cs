//Final POE - Junaid.K
//ST10141404@rcconnect.edu.za

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Xpf.Charts;

namespace FinalPOE
{
    public partial class MainWindow : Window
    {
        private RecipeManager recipeManager;
        private List<Recipe> filteredRecipes;

        public MainWindow()
        {
            InitializeComponent();
            recipeManager = new RecipeManager();
            filteredRecipes = new List<Recipe>();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            var addRecipeWindow = new AddRecipeWindow(recipeManager);
            addRecipeWindow.ShowDialog();
        }

        private void DisplayRecipeList_Click(object sender, RoutedEventArgs e)
        {
            recipeListGroupBox.Visibility = Visibility.Visible;
            recipeListBox.ItemsSource = recipeManager.GetRecipeNames();
        }

        private void IngredientFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredientFilter = ingredientFilterTextBox.Text.ToLower();
            filteredRecipes = recipeManager.FilterRecipesByIngredients(ingredientFilter);
            recipeListBox.ItemsSource = filteredRecipes.Select(r => r.Name);
        }

        private void FoodGroupFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string foodGroupFilter = foodGroupFilterTextBox.Text.ToLower();
            filteredRecipes = recipeManager.FilterRecipesByFoodGroup(foodGroupFilter);
            recipeListBox.ItemsSource = filteredRecipes.Select(r => r.Name);
        }

        private void MaxCaloriesFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(maxCaloriesFilterTextBox.Text, out int maxCalories))
            {
                filteredRecipes = recipeManager.FilterRecipesByMaxCalories(maxCalories);
                recipeListBox.ItemsSource = filteredRecipes.Select(r => r.Name);
            }
            else
            {
                MessageBox.Show("Invalid max calories value. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            ingredientFilterTextBox.Clear();
            foodGroupFilterTextBox.Clear();
            maxCaloriesFilterTextBox.Clear();
            filteredRecipes.Clear();
            recipeListBox.ItemsSource = recipeManager.GetRecipeNames();
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recipeListBox.SelectedItem != null)
            {
                string selectedRecipeName = recipeListBox.SelectedItem.ToString();
                var selectedRecipe = filteredRecipes.FirstOrDefault(r => r.Name == selectedRecipeName) ?? recipeManager.GetRecipeByName(selectedRecipeName);

                ingredientListBox.ItemsSource = selectedRecipe.Ingredients;
                stepListBox.ItemsSource = selectedRecipe.Steps;
                recipeGroupBox.Visibility = Visibility.Visible;
            }
            else
            {
                recipeGroupBox.Visibility = Visibility.Collapsed;
            }
        }

        private void FoodGroupPieChartButton_Click(object sender, RoutedEventArgs e)
        {
            if (filteredRecipes.Any())
            {
                var foodGroupPercentages = recipeManager.CalculateFoodGroupPercentages(filteredRecipes);
                foodGroupPieChart.DataContext = foodGroupPercentages;
                foodGroupPieChart.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("No filtered recipes found. Please apply filters and try again.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
    }

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

        public List<string> GetRecipeNames()
        {
            return recipes.Select(r => r.Name).ToList();
        }

        public Recipe GetRecipeByName(string name)
        {
            return recipes.FirstOrDefault(r => r.Name == name);
        }

        public List<Recipe> FilterRecipesByIngredients(string ingredientFilter)
        {
            return recipes.Where(r => r.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientFilter))).ToList();
        }

        public List<Recipe> FilterRecipesByFoodGroup(string foodGroupFilter)
        {
            return recipes.Where(r => r.Ingredients.Any(i => i.FoodGroup.ToLower().Contains(foodGroupFilter))).ToList();
        }

        public List<Recipe> FilterRecipesByMaxCalories(int maxCalories)
        {
            return recipes.Where(r => r.Ingredients.Sum(i => i.Calories) <= maxCalories).ToList();
        }

        public List<FoodGroupPercentage> CalculateFoodGroupPercentages(List<Recipe> recipes)
        {
            var foodGroupPercentages = new List<FoodGroupPercentage>();
            var foodGroupCounts = new Dictionary<string, int>();

            foreach (var recipe in recipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (foodGroupCounts.ContainsKey(ingredient.FoodGroup))
                        foodGroupCounts[ingredient.FoodGroup]++;
                    else
                        foodGroupCounts[ingredient.FoodGroup] = 1;
                }
            }

            int totalIngredients = recipes.SelectMany(r => r.Ingredients).Count();
            foreach (var foodGroupCount in foodGroupCounts)
            {
                double percentage = (foodGroupCount.Value / (double)totalIngredients) * 100;
                foodGroupPercentages.Add(new FoodGroupPercentage { FoodGroup = foodGroupCount.Key, Percentage = percentage });
            }

            return foodGroupPercentages;
        }
    }

    public class FoodGroupPercentage
    {
        public string FoodGroup { get; set; }
        public double Percentage { get; set; }
    }
}

