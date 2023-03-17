# Natural selection AI

# Machine learning in the task of agent-based modeling

Unity/C#

Presentation in Russian: [here](https://docs.google.com/presentation/d/1_z9vxqRSE8IVE3WKkVb-bqAoDB3aCd0E65gc9ajG0UE/edit?usp=sharing)
Document: [here](https://docs.google.com/document/d/1CnagabWTiOvTV-i7GdWdpSsiXP-CcpaU/edit?usp=sharing&ouid=108282425598754075132&rtpof=true&sd=true)

![Alt Text](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExODVmOWZkZWRiNzI4ODVhYjIzNjdjYjE2NDAwOGU4NWQxMWZjMjM2MyZjdD1n/QQzRVgWJ7b8T1fUaOd/giphy.gif)

About 10 simulations were run with data collection for a total duration of about 60 hours.

The dynamics of the decline and growth of populations in the environment, and their influence on each other, is traced.

In this model, the algorithms have successfully adapted to environmental conditions, agents have learned to survive among other agents for quite a long time.

### Schematic description of a neural network

![Alt Text](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExNDg2MGNmMTNhZTNmZjg0NzQ1Y2FmZmM5N2I2MWUyYTVjYWQ0YWE2YiZjdD1n/HlMChJJqSpPJ3Ke8WU/giphy.gif)

Each agent has this neural network. The lengths of 4 vectors directed from the agent to the largest accumulation of food, predatory agents, herbivorous agents and protected agents are fed to the network input.

The output values of the network are converted into a 2-dimensional vector, which is then used to move the agent.

The neural network is trained according to the principle of natural selection: only the fittest agents survive.
They pass their neural network weights on to their offspring with some probability of mutation. (10 percent chance of weight mutation)

Thus, sampling and natural selection are carried out, where with each step of the simulation, only the most adapted agents to the environment survive.

### data for 300 thousand simulation steps (approximately 4 hours)

![Alt Text](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExODNkNTcyMjM5MDgzNWMxOWY3Mjk3M2RhY2I0MjAxODNmYWQ1MGJkZSZjdD1n/82N2YQtxL2Wgc2DAEd/giphy.gif)

The Predatory Pattern became a record for life expectancy.
Such an agent managed to live in the environment for 6 hours.

The second main group is protective agents. (Blue colour)
Average age of an agent in the environment: approximately 65 seconds, but individuals have also been observed that live in the environment for an hour or more.

During his life, an agent finds food, leaves offspring, attacks other agents if he and his ancestors contained a predatory behavior model.
Or maneuvers away from other agents if its pattern is defensive.

As a result, an environment has been obtained in which agents operate, multiply and develop, controlled by decentralized algorithms.
