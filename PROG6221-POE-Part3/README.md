# Programming-Module---PROG6221POE---ST10141404ROG
This the repository for my PROG6221 POE Part 2

This implementation consists of several classes: Ingredient, Step, Recipe, RecipeManager, and the Program class which contains the main entry point of the application.

The Recipe class represents a single recipe and contains properties for the recipe name, ingredients, and steps. It also provides methods to add ingredients and steps, scale the recipe, and display the recipe. The Recipe class raises the RecipeCaloriesExceeded event when the total calories of the recipe exceed 300.

The RecipeManager class manages a collection of recipes and provides methods to add recipes, display the recipe list, and retrieve a recipe by name.

The Program class contains the main method and handles user input. It allows the user to add recipes, display the recipe list, select a recipe to display, scale a recipe, and reset quantities. It also includes a handler for the RecipeCaloriesExceeded event.
