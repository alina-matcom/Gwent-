effect {
  Name: "Damage",
  Params: {
    Amount: Number
  },
  Action: (targets, context) => {
    for target in targets {
      i = 0;
      while (i++ < Amount)
        target.Power -= 1;
    };
  };
}

card {
  Type: "Oro",
  Name: "Beluga",
  Faction: "Northern Realms",
  Power: 10,
  Range: ["Melee", "Ranged"],
  OnActivation: [
    {
      Effect: {
        Name: "Damage",
        Amount: 2
      },
      Selector: {
        Source: "board",
        Single: false,
        Predicate: (unit) => unit.Faction == "Northern Realms"
      },
    }
  ]
}
card {
  Type: "Oro",
  Name: "Griffin",
  Faction: "Monsters",
  Power: 8,
  Range: ["Melee", "Siege"],
  OnActivation: [
    {
      Effect: {
        Name: "Damage",
        Amount: 2
      },
      Selector: {
        Source: "board",
        Single: false,
        Predicate: (unit) => unit.Faction == "Monsters"
      },
    }
  ]
}
card {
  Type: "Plata",
  Name: "Triss Merigold",
  Faction: "Northern Realms",
  Power: 7,
  Range: ["Ranged", "Siege"],
  OnActivation: [
    {
      Effect: {
        Name: "Damage",
        Amount: 2
      },
      Selector: {
        Source: "board",
        Single: false,
        Predicate: (unit) => unit.Faction == "Northern Realms"
      },
    }
  ]
}

card {
  Type: "Plata",
  Name: "Yennefer",
  Faction: "Northern Realms",
  Power: 9,
  Range: ["Ranged", "Siege"],
  OnActivation: [
    {
      Effect: {
        Name: "Damage",
        Amount: 2
      },
      Selector: {
        Source: "board",
        Single: false,
        Predicate: (unit) => unit.Faction == "Northern Realms"
      },
    }
  ]
}

card {
  Type: "Oro",
  Name: "Geralt of Rivia",
  Faction: "Neutral",
  Power: 12,
  Range: ["Melee", "Siege"],
  OnActivation: [
    {
      Effect: {
        Name: "Damage",
        Amount: 2
      },
      Selector: {
        Source: "board",
        Single: false,
        Predicate: (unit) => unit.Faction == "Neutral"
      },
    }
  ]
}