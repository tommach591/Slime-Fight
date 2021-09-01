using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLore : MonoBehaviour
{
    /* 

    To create entry:

        "public static string nameStory = @" then type story

        "public static string namePrime = @" then type recommended primary stats (health, attack, defense)

        "public static string nameCrit = @" then type recommended crit stats (crit rate, crit damage)

        Add to Dictionary (AKA map), using the name of the monster as the "key" and your string you wrote as "value".

        DONT FORGET TO ADD COMMA!!!
    
    */
    public Dictionary<string, string> story = new Dictionary<string, string>() {
        {"No Target", noStory},
        {"Jolly", jollyStory},
        {"Apex Jolly", apexJollyStory},
        {"Bee", beeStory},
        {"Tomato", tomatoStory},
        {"Crab", crabStory},
    };

    public Dictionary<string, string> prime = new Dictionary<string, string>() {
        {"No Target", noPrime},
        {"Jolly", jollyPrime},
        {"Apex Jolly", apexJollyPrime},
        {"Bee", beePrime},
        {"Tomato", tomatoPrime},
        {"Crab", crabPrime},
    };

    public Dictionary<string, string> crit = new Dictionary<string, string>() {
        {"No Target", noCrit},
        {"Jolly", jollyCrit},
        {"Apex Jolly", apexJollyCrit},
        {"Bee", beeCrit},
        {"Tomato", tomatoCrit},
        {"Crab", crabCrit},
    };

/* No Target ********************************************************************************************************************/
public static string noStory = 
@"No Target";

public static string noPrime = 
@"Health - ???
Attack - ???
Defense - ???";

public static string noCrit = 
@"Crit Rate - ???
Crit Damage - ???";
    
/* Jolly ***********************************************************************************************************************/
public static string jollyStory =
@"Jollys like to live their life peacefully sniffing flowers and eating grass.";

public static string jollyPrime = 
@"Health - 20
Attack - 1
Defense - 0";

public static string jollyCrit = 
@"Crit Rate - 5.00%
Crit Damage - 1.50x";

/* ApexJolly ***********************************************************************************************************************/
public static string apexJollyStory =
@"Something is off about this Jolly...

Almost as if all the life in it has been stripped away...

It emits a strange black mist...

Huh...? I can sense something from it... Fear? 

Or maybe sadness...?";

public static string apexJollyPrime = 
@"Health - 35
Attack - 20
Defense - 25";

public static string apexJollyCrit = 
@"Crit Rate - 20.00%
Crit Damage - 1.75x";

/* Bee *************************************************************************************************************************/
public static string beeStory = 
@"After witnessing the villagers hunt their family, Jolly bee-came angry.

This anger caused Jolly to grow horns, giving it the ability to summon more Jollys.";

public static string beePrime = 
@"Health - 20
Attack - 5
Defense - 5";

public static string beeCrit = 
@"Crit Rate - 7.50% 
Crit Damage - 1.50x";

/* Tomato *************************************************************************************************************************/
public static string tomatoStory = 
@"What the...?

How did this tomato grow so big?! And it's sentient?!

Now it's attacking the farmers before it can be harvested!";

public static string tomatoPrime = 
@"Health - 25
Attack - 15
Defense - 10";

public static string tomatoCrit = 
@"Crit Rate - 10.00% 
Crit Damage - 1.75x";

/* Crab *************************************************************************************************************************/
public static string crabStory = 
@"Giant Crab with great strength!

These monsters throw water shurikens with its powerful claws.

They are extremely agile!

Every festival, Crabs are hunted for their prized meat for the villagers to feast on.";

public static string crabPrime = 
@"Health - 25
Attack - 15
Defense - 10";

public static string crabCrit = 
@"Crit Rate - 10.00% 
Crit Damage - 1.75x";

/* New ***********************************************************************************************************************/

}
