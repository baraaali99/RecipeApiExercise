using System.Text;
using Spectre.Console;
using RecipeConsole.Client;
internal class ConsoleUi
{
    static List<string> categoryList = new List<string>();
    public static Recipe AddRecipe(List<string> catList)
    {
        catList = categoryList;
        Recipe recipe = new Recipe();
        var title = AnsiConsole.Ask<string>("What is the [green]recipe[/] called?");
        recipe.Title = title;
        var ingredients = new List<string>();
        var instructions = new List<string>();

        AnsiConsole.MarkupLine("Enter all the [green]ingredients[/]. [red] after you're done of writing instructions press space to move to next step [/]");
        var ingredient = AnsiConsole.Ask<string>("Enter recipe ingredient: ");
        while (ingredient != "")
        {
            recipe.Ingredients.Add(ingredient);
            ingredient = AnsiConsole.Prompt(new TextPrompt<string>("Enter recipe ingredients: ").AllowEmpty());
        };

        AnsiConsole.MarkupLine("Enter all the [green]instructions[/]. [red] after you're done of writing ingredients press space to move to next step [/]");
        var instruction = AnsiConsole.Ask<string>("Enter [green]recipe[/] instructions: ");
        while (instruction != "")
        {
            recipe.Instructions.Add(instruction);
            instruction = AnsiConsole.Prompt(new TextPrompt<string>("Enter [green]recipe[/] instructions: ").AllowEmpty());

        };

        if (categoryList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Threr are no categories so please add Categories your recipes can belong to[/]");
            recipe.Categories.Add("[red]not assigned to specific Category yet[/]");// here the recipe the user added won't belong to any category
            return recipe;
        }
        else
        {
        var selectedcategories = AnsiConsole.Prompt(
        new MultiSelectionPrompt<String>()
        .PageSize(10)
        .Title(" Which [white]categories[/] does this recipe belong to?")
        .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
        .InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
        .AddChoices(categoryList));

            recipe.Categories = selectedcategories;
            return recipe;
        }
        return recipe;
    }
    public static void ListRecipes(List<Recipe> recipesList)
    {
        var table = new Table();
        table.AddColumn("Recipe Name");
        table.AddColumn("Ingredients");
        table.AddColumn("Instructions");
        table.AddColumn("Categories");

        foreach (var recipe in recipesList)
        {
            int ingCnt = 0;
            var ingredients = new StringBuilder();
            foreach (String ingredient in recipe.Ingredients)
            {
                ingCnt++;
                ingredients.Append(ingCnt + "-" + "[gray]" + ingredient + "[/]" + "\n");
            }

            int insCnt = 0;
            var instructions = new StringBuilder();
            foreach (String instruction in recipe.Instructions)
            {
                insCnt++;
                instructions.Append(insCnt + "-" + "[gray]" + instruction + "[/]" + "\n");
            }

            var categories = new StringBuilder();
            foreach (String category in recipe.Categories)
            {
                categories.Append("-" + "[gray]" + category + "[/]" + "\n");
            }

            table.AddRow(recipe.Title, ingredients.ToString(), instructions.ToString(), categories.ToString());
        }

        AnsiConsole.Write(table);
    }
    public static Recipe EditRecipe(List<Recipe> recipesList, List<string> categoriesList)
    {
        categoriesList = categoryList;
        if (recipesList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There are no recipes to [/]");
            return null;
        }

        var chosenRecipe = AnsiConsole.Prompt(
           new SelectionPrompt<Recipe>()
               .Title("Which Recipe would you like to edit?")
               .AddChoices(recipesList));

        var command = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
               .Title("What would you like to do?")
               .AddChoices(new[]
               {
           "Edit title",
           "Edit Ingredients",
           "Edit Instructions",
           "Edit Categories"
               }));

        AnsiConsole.Clear();
        switch (command)
        {
            case "Edit title":
                chosenRecipe.Title = AnsiConsole.Ask<string>("What is the [green]recipe[/] called?");
                break;
            case "Edit Ingredients":
                chosenRecipe.Ingredients.Clear();
                AnsiConsole.MarkupLine("Enter all the [green]ingredients[/]. [red] after you're done of writing instructions press space to move to next step [/]");
                var ingredient = AnsiConsole.Ask<string>("Enter ingredient: ");
                while (ingredient != "")
                {
                    chosenRecipe.Ingredients.Add(ingredient);
                    ingredient = AnsiConsole.Prompt(new TextPrompt<string>("Enter ingredient: ").AllowEmpty());
                };
                break;
            case "Edit Instructions":
                chosenRecipe.Instructions.Clear();
                AnsiConsole.MarkupLine("Enter all the [green]instructions[/]. [red] after you're done of writing instructions press space to move to next step [/]");
                var instruction = AnsiConsole.Ask<string>("Enter instruction: ");
                while (instruction != "")
                {
                    chosenRecipe.Instructions.Add(instruction);
                    instruction = AnsiConsole.Prompt(new TextPrompt<string>("Enter instruction: ").AllowEmpty());
                };
                break;
            case "Edit Category":
                var selectedcategories = AnsiConsole.Prompt(
                new MultiSelectionPrompt<String>()
                .PageSize(10)
                .Title("Which [green]category[/] does this recipe belong to?")
                .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
                .InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to choose the category you toggeled)[/]")
                .AddChoices(categoryList));

                chosenRecipe.Categories = selectedcategories;
                break;
        }
        return chosenRecipe;
    }
    public static string AddCategory()
    {
        AnsiConsole.MarkupLine("Enter all the [green]categories[/]. [red] after you're done of writing categories press space to move to next step [/]");
        string category =  AnsiConsole.Ask<string>("What is the [green]category[/] called?");
        categoryList.Add(category);
        return category;
    }
    public static List<string> ChooseCategories(List<string> categoriesList)
    {
        categoriesList = categoryList;
        if (categoriesList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Threr are no categories so please add Categories your recipes can belong to[/]");
            return categoriesList;
        }
        var selectedcategories = AnsiConsole.Prompt(
         new MultiSelectionPrompt<String>()
         .PageSize(10)
         .Title(" Which [white]categories[/] does this recipe belong to?")
         .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
         .InstructionsText("[grey](Press [blue]Space[/] to toggle a category, [green]Enter[/] to accept)[/]")
         .AddChoices(categoriesList));

        return selectedcategories;
    }
    public static string EditCategory(List<Recipe> recipesList, List<string> categoriesList)
    {
        categoriesList = categoryList;
        if (categoriesList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There are no Categories to be edited[/]");
            return null;
        }

        var chosenCategory = AnsiConsole.Prompt(
        new SelectionPrompt<string>().Title("Which Category would you like to edit?").AddChoices(categoriesList));

        String newCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("What would you like to change the name to?"));

        categoriesList.Remove(chosenCategory);
        categoriesList.Add(newCategoryName);
        int i = 0;
        foreach (var r in recipesList)
        {
            if (i < recipesList.Count && i < recipesList[i].Categories.Count && r.Categories[i] == recipesList[i].Categories[i])
            {
                r.Categories[i] = newCategoryName;
            }
            i++;
        }
        return chosenCategory;
    }
    public static List<Recipe> ChooseRecipes(List<Recipe> recipesList)
    {
        if (recipesList.Count == 0)
        {
            AnsiConsole.MarkupLine("There are no Recipes");
            Environment.Exit(0);
        }
        var selectedRecipes = AnsiConsole.Prompt(
        new MultiSelectionPrompt<Recipe>()
        .PageSize(10)
        .Title("Which [green]recipes[/] does this recipe belong to?")
        .InstructionsText("[grey](Press [blue]Space[/] to toggle a recipe, [green]Enter[/] to accept)[/]")
        .AddChoices(recipesList));

        return selectedRecipes;
    }
    public static string ChooseCategory(List<string> categoriesList)
    {
        if (categoriesList.Count == 0)
        {
            AnsiConsole.WriteLine("There are no categories!");
            Environment.Exit(0);
        }
        var chosenCategory = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
               .Title("Which Category would you like to edit?")
               .AddChoices(categoriesList));

        return chosenCategory;
    }
}