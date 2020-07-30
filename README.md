# Stat System Foundation
Stat System Foundation is a starting point for adding a stat and buff/debuff system to your project. It requires that you write code and is feature-light. Most "how to make a stat system" guides for Unity are bulky and class-exploded messes (according to my personal taste). This aims to fix that by being a bit more data-driven and work within design constraints.

The API has "health" and "am I dead?" built in. You might want to rip that out, if so it's easy. Just "find-references" to HP and MaxHP and remove those lines of code.

## The Algorithm

There are **base stats**. Then there are **modified stats** which are stats after **buffs/debuffs** are applied.
Modified Stats are calculated using **((BaseStats + AddPreMult) * Mult) + AddPostMult**.
* **BaseStats** are defined in editor. It's the base-stat.
** This is used for your HP. It can be subtracted and added to, without using buffs since damage persists.
* **AddPreMult** is a value added to the base-stat before multiplication. This scaled by the multiplier alongside the base-stat. 
** This could be a modificaiton to base-stats. A ring that increases your faith or max-hp.
* **Mult** is a multiplier. It multiplies the current value plus the AddPreMult. 
** This could be for weapon-scaling or a curse that halves your max-hp.
* **AddPostMult** is a value added to the stat after multiplication. This is not scaled by the modifier.
** This could be a curse that halves reduces your HP but restores the health after a duration.

### Buffs
Buffs have a few traits: **StackableSetting**, **TargetVariable**, **ExpirationType**, and **TargetStat**.
* **StackableSetting** Can the buff stack? This could be used to ensure that only one buff of this type is applied.
* **TargetVariable** This defines which step of the algorithm the buff targets.
* **ExpirationType** How the buff expires. Only "timed" is provided.
* **TargetStat** Which stat the buff modifies..


## How to use
[Get a .unitypackage from the releases page.](https://github.com/godjammit/Stat-System-Foundation/releases) Modify and get rolling with your own project from there. 

It isn't much code, I'd reccomend giving it a read and familiarize yourself with it. Here are some places to look...
* EntityStats.cs - Switch statements inside of the FixedUpdate method
* EntityStats.cs - The "PUT POST-BUFF LOGIC HERE" portion of the FixedUpdate 
* EntityStats.cs - The Stat enum
* Buff.cs - All enums inside this file.
