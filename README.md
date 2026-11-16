# Damage System

Almost all games have a way for objects to inflict damage into other ones and recover from them using some item. The damage is usually shown by some UI and/or some effect during gameplay. Also, it's a common practice to have a time window between each damage hit so one cannot inflict too much damage in a small succession window.

This package adds Damage System scripts which has all those features.

## How To Use

There are 3 main components on this system: Damager, Damageable and Energy.

The Damager component can damage Damageable components, reducing the current value from Energy. A Damageable GameObject may have an Energy component and this can be shown in the UI by using an EnergyBar component. 

You may have an Enemy GameObject with a Damager, Damageable and Energy components, so it can damage the Player and be damaged by the Players attacks. Also, you can show the Enemy energy using a EnergyBar (in a Screen or World Space Canvas).

Furthermore, events are available so other scripts can easily get notified when some actions are executed, like when the energy ends, increases etc.

### Setup Checklist

1. Add a `Damageable` component to the target GameObject.
2. Assign its `Collider` field.
3. Optionally add an `Energy` component (recommended for health-like behavior).
4. Add a `Damager` to the attacker GameObject.
5. Assign the `Damager` `Collider`, configure `Layers` and `CurrentAmount`.

### Core Runtime Flow

- `Damager` checks nearby colliders in `FixedUpdate` using `Physics.OverlapBoxNonAlloc` and its own collider bounds.
- For each hit that implements `IDamageable`, it calls `TryInflictDamage`.
- `Damageable.TryTakeDamage` only accepts damage when enabled and not invulnerable.
- If `Damageable` has `Energy`, damage removes `damager.CurrentAmount` from `Energy.Current`.
- When energy reaches zero, `Damageable` disables itself.

### Components Overview

#### Damager

`Damager` (`MonoBehaviour`, implements `IDamager`) is the default area/contact damager.

Main fields:
- `Collider`: collider used as overlap shape source.
- `CurrentAmount`: clamped to `>= 0`.
- `Layers`: target layers to damage.
- `DisableAfterInflictDamage`: disables the damager GameObject after a successful hit.

Main event:
- `OnDamageInflicted(IDamageable)`.

#### RayDamager

`RayDamager` is a non-`MonoBehaviour` implementation of `IDamager`.

Use `TryHit(...)` to raycast and inflict damage when the hit target has `IDamageable`.

#### Damageable

Default `IDamageable` implementation.

Main fields:
- `Energy`: optional. If null, it still receives damage events without energy depletion.
- `Collider`: used by damagers/intersection logic.
- `IsInvulnerable`: blocks incoming damage.

Main event:
- `OnDamageTaken(IDamager)`.

Utilities:
- `Respawn()` re-enables component and refills energy to `Energy.Initial`.

#### Energy

`Energy` represents a generic resource (health, stamina, shield, etc).

Main properties:
- `Initial`
- `Max`
- `Current`

Main methods:
- `FillEnergy()`
- `FillEnergyByHalf()`
- `ResetEnergy()`
- `Add(float)`
- `Remove(float)`
- `Complete()`
- `RemoveAll()`
- `SetWithoutNotify(float)`

Main events:
- `OnEnergyChanged`
- `OnEnergyEnded`
- `OnEnergyCompleted`

#### DamageableFeedback

Visual blink feedback by changing child renderer material colors for a frame-based animation window.

During playback it sets `Damageable.IsInvulnerable = true`, then restores it to `false` at the end.

#### DamageableFallback

Applies a short knockback-like move away from the attacker transform when damage is taken.

Main events:
- `OnMoveStarted`
- `OnMoveFinished`

#### DamagerIntersector

Listens to `Damager.OnDamageInflicted` and emits a computed intersection point:
- `OnIntersected(Vector3 point, IDamageable damageable)`

#### EnergyUIBar (optional)

Available only when `com.unity.ugui` is present (`PACKAGE_UGUI` define).

Binds an `Energy` component to a `Slider` and updates value/color through a `Gradient`.

### Notes

- This package assembly is `OneM.DamageSystem`.
- It references `OneM.Attributes`.
- `EnergyUIBar` is conditionally compiled and requires UGUI.

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
