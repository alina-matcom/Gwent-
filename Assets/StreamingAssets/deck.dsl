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
  Faction: "Northern" @@ "Realms",
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
        Predicate: (unit) => unit.Faction == "Northern" @@ "Realms"
      },
    }
  ]
}
