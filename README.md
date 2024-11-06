# Vpokefficency

 
The above pictures shows the architecture used in the Vpokefficency solution. 
# VPokefficiency Solution Architecture
## The VPokefficiency solution architecture consists of six primary layers:
1.	**Presentation Layer (UI)**: This layer is responsible for handling user interaction, validating input and displaying results back to end user. It communicates with the Service Layer.
2.	**Service Layer**: This layer contains the business logic and orchestrates the flow of data. It interacts with the Gateway Layer to fetch data and then processes it before passing it back to the Presentation Layer.
3.	**Gateway Layer**: This layer handles communication with external PokeAPI, to fetch necessary data. It then passes this data to the Service Layer.
4.	**Domain Layer**: This layer defines the core domain objects. It serves as the foundation for the other layers and has no external dependencies except for the Framework Layer.
5.	**Framework Layer**: This layer provides cross-cutting concerns like utility functions, It is used by all other layers.
6.	**Bootstrap Layer**: This layer initializes the application and sets up the necessary components, such as dependency injection and configuration.
## Layer Dependencies:
*	The **Domain Layer** is the foundation and depends only on the Framework Layer.
*	The **Gateway Layer** depends on the Domain Layer and the Framework Layer.
*	The **Service Layer** depends on the Gateway Layer and the Framework Layer.
*	The **Presentation Layer** depends on the Service Layer and the Framework Layer.
*	All layers depend on the Framework Layer for shared utilities.
  
By separating concerns into distinct layers, the architecture helps with modularity, testability, and maintainability
Outside of these six layers, a separate testing project is dedicated to unit testing

## Running the application
Open your command prompt or terminal, navigate to the directory where the executable is located, and type the following command:

> Vpokefficency.exe

The application prompts the user to enter a Pokemon name in the console. Once a valid Pokémon name is provided, it displays the Pokemon's effectiveness against various types in the console output.

## Exiting the application
To exit the application, simply type "quit" (without quotes) in the console when prompted for a Pokemon name. The application will then terminate.

## Processing Pokemon Type Effectiveness Data
I processed the JSON data retrieved from the PokéAPI to create a list of TypeEffectiveness objects. Each TypeEffectiveness object represents a type and its effectiveness against other types.

### TypeEffectiveness Class:
```
public class TypeEffectiveness
{
    public string Name { get; set; }
    public bool IsStrong { get; set; }
    public bool IsWeak { get; set; }
}
```
## Processing the Data:

As I iterated through the different types and their damage relations, I added TypeEffectiveness objects to a list. To avoid duplicates, I used the Union method on the list.

### Handling Duplicate and Ambiguous Types:

+ **Absolute Duplicates**: Objects with identical Name and IsStrong/IsWeak values were considered duplicates and removed using Union.
  
+ **Ambiguous Types**: For types that were both strong and weak against a specific type (e.g., Normal vs. Ghost), a combined TypeEffectiveness object was created to represent this ambiguity.
For example, if a type was both strong and weak against another type, the resulting TypeEffectiveness object would look like this:
```
{ Name = "Ghost", IsStrong = true, IsWeak = true }
```

This approach effectively captures the complex nature of Pokémon type matchups, including both strong and weak relationships.
