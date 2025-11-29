# Copilot Instructions for RimWorld Modding Project: Picnic Area

## Mod Overview and Purpose

The Picnic Area mod introduces an outdoor picnic area for pawns in RimWorld, allowing them to eat outside without incurring the "Ate without table" debuff. Inspired by the Picnic Spot mod by cucumpear, this mod enhances the dining experience of pawns while taking into account weather conditions and time settings. It gives players more control over where and when their pawns can enjoy a meal, adding a layer of complexity and immersion to the game.

## Key Features and Systems

- **Weather and Time Settings**: The mod includes settings for players to control when pawns will picnic, based on time of day and temperature. By default, pawns avoid picnicking in poor weather but this can be adjusted via settings.
- **Seating Requirement**: The picnic area needs to have seating, but does not require tables, reflecting a true picnic scenario.
- **Area Preferences**: When both picnic areas and regular dining areas are available, there is a 50% chance pawns will choose to eat at the picnic area. If all conditions for picnicking are met and there are no dining tables available, pawns will always opt for the picnic spot.
- **Multilingual Support**: Chinese translations have been added by a contributor, HawnHan, making the mod accessible to a broader audience.

## Coding Patterns and Conventions

- **Class Naming**: Classes are named succinctly, corresponding directly to their responsibilities (e.g., `PicnicArea`, `Zone_PicnicArea`).
- **Access Modifiers**: Public and internal access modifiers are used carefully to encapsulate functionality.
- **Static Methods**: Utilized for utility-like functions that need to be accessed globally, such as in `GenGrid_HasEatSurface`.

## XML Integration

- Integration with RimWorld's def-driven XML system allows the mod to define and update custom game elements easily.
- XML files are used to define settings, thought definitions, and translations.
- Ensures that any changes to gameplay elements through the mod are consistent and maintain compatibility with the base game mechanics.

## Harmony Patching

- **Purpose**: Harmony is utilized to patch existing game methods, integrating mod functionalities seamlessly without altering the original game code directly.
- **Implementation**: Focus on patching methods related to pawns selecting eating spots and managing memories related to dining.
- **Examples**: Classes such as `MemoryThoughtHandler_TryGainMemory` demonstrate the use of static methods to adjust game behaviors.

## Suggestions for Copilot

To enhance development efficiency and maintain code quality, the following suggestions are recommended for using GitHub Copilot:

- **Encourage Code Reuse**: Leverage existing patterns in the mod's codebase. When suggesting new functionality, consider if existing methods or classes can be expanded.
- **Follow Conventions**: Maintain the naming conventions and coding patterns outlined, particularly around class and method declarations. 
- **Suggest Contextual Code**: When providing suggested completions, incorporate the mod-specific terminology and relevant method calls fitting to the Picnic Area's theme.
- **Highlight XML Updates**: Suggest XML structure definitions when new settings or definitions are created, ensuring they align with existing configurations.
- **Aid in Harmony Extensions**: Encourage comprehensive Harmony patches by suggesting prefix, postfix, and transpiler methods where applicable to extend and alter base game logic interactively.

By adhering to these patterns and suggestions, Copilot can be a seamlessly integrated tool that bolsters the mod development process in RimWorld.
