# Damage System

Almost all games have a way for objects to inflict damage into other ones and recover from them using some item. The damage is usually shown by some UI and/or some effect during gameplay. Also, it's a common practice to have a time window between each damage hit so one cannot inflict too much damage in a small succession window.

This package adds Damage System scripts which has all those features.

## How To Use

There are 3 main components on this system: Damager, Damageable and Energy.

The Damager component can damage Damageable components, reducing the current value from Energy. A Damageable GameObject may have an Energy component and this can be shown in the UI by using an EnergyBar component. 

You may have an Enemy GameObject with a Damager, Damageable and Energy components, so it can damage the Player and be damaged by the Players attacks. Also, you can show the Enemy energy using a EnergyBar (in a Screen or World Space Canvas).

Furthermore, events are available so other scripts can easily get notified when some actions are executed, like when the energy ends, increases etc.

### Using [...]


## Installation

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set and the correct git credentials to 1M Bits Horde.

- In this repo, go to Code button, select SSH and copy the URL.
- In Unity, use the **Package Manager** "Add package from git URL..." feature and paste the URL.
- Set the version adding the suffix `#[x.y.z]` at URL

---

**1 Million Bits Horde**

[Website](https://www.1mbitshorde.com) -
[GitHub](https://github.com/1mbitshorde) -
[LinkedIn](https://www.linkedin.com/company/1m-bits-horde)
