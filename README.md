# Dynamics of UAV Impacts Using a Physics Engine

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black.svg?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-blue.svg?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![MATLAB](https://img.shields.io/badge/MATLAB-R2024-red.svg?logo=mathworks)](https://www.mathworks.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

MEng First-Class Honours research dissertation at The University of Manchester investigating dynamic high-energy collision mechanics between rotary-wing Unmanned Aerial Vehicles (UAVs) and bio-fidelic human subjects using high-fidelity physics engines.

---

## Investigation Overview
Evaluating the crashworthiness and trauma severity of low-altitude UAV operations under human-collision scenarios. The environment models high-speed kinetic energy transfer, structural collapse dynamics, and trauma criteria (Head Injury Criterion — HIC) across variable anatomical joint stiffness profiles.

* **Impact Baseline:** 3.5 kg rotary-wing UAV operating under active closed-loop PID control at 16 m/s terminal velocity.
* **Target Subject:** 75 kg multi-body bio-fidelic human model with anthropometric mass and inertia distributions.
* **Biomechanical Modeling:** Anatomical Range of Motion (ROM) limiters across sagittal, frontal, and transverse planes with variable angular spring stiffness to evaluate passive vs. active muscle bracing.

---

## Key Findings
1. **Primary Impact Severity:** Active muscular bracing and joint stiffness do not mitigate initial contact forces or peak kinetic energy transfer during primary torso impacts.
2. **Secondary Whiplash Mitigation:** Active stiffness significantly suppresses secondary post-collision head/neck whiplash and violent extremity flail, reducing secondary blunt trauma.
3. **Multi-Body Mass Distribution:** Increasing ragdoll kinematic fidelity delays total structural collapse while redistributing shock loads across peripheral joints.

---

## Repository Architecture

```text
uav-impact-dynamics/
├── unity/
│   ├── scripts/
│   │   ├── Drone.cs               # Dynamic flight physics & collision event handling
│   │   ├── Simple_uav.cs          # Simplified kinematic baseline model
│   │   ├── PID.cs                 # Attitude & velocity regulation loops
│   │   ├── RagdollComplexity.cs   # Multi-body ragdoll hierarchy management
│   │   └── RagdollInformation.cs  # High-frequency telemetry exporter (CSV streaming)
│   └── prefabs/
│       ├── UAV 1.prefab           # 3.5 kg quadcopter airframe and collision mesh
│       ├── Neck Ragdoll.prefab    # High-fidelity cervical spine test model
│       └── Ragdoll X1..X12.prefab # Multi-body ragdoll architectures (1 to 12 DOFs)
└── matlab/
    ├── RagdollComplexityData.m    # Acceleration, jerk, and kinetic energy processing
    └── NeckTestingData.m          # Angular deflection & Head Injury Criterion (HIC) solver
```

---

## Analytical Methodology

### Head Injury Criterion (HIC) Formulation
Trauma severity is computed from linear acceleration profiles extracted at millisecond resolution:

$$HIC = \max_{t_1, t_2} \left[ (t_2 - t_1) \left( \frac{1}{t_2 - t_1} \int_{t_1}^{t_2} a(t) \, dt \right)^{2.5} \right]$$

Where:
- $a(t)$ is the resultant head acceleration measured in gravitational units ($g$).
- The time interval $(t_2 - t_1)$ is evaluated over a maximum window of $15\text{ ms}$ ($HIC_{15}$) and $36\text{ ms}$ ($HIC_{36}$).

---

## Getting Started

### Prerequisites
* **Unity 2022.3 LTS** or higher
* **MATLAB R2022b** or higher

### Running the Simulation
1. Clone the repository:
   ```bash
   git clone [https://github.com/ewanb4/uav-impact-dynamics.git](https://github.com/ewanb4/uav-impact-dynamics.git)
   ```
2. Open the project in **Unity Hub** and select the main collision staging scene.
3. Attach the desired human target prefab (`Ragdoll X1.prefab` through `Ragdoll X12.prefab`) into the target fixture slot.
4. Execute Play mode; telemetry will output to `unity/telemetry_output.csv`.
5. Run `matlab/RagdollComplexityData.m` to generate kinematic deflection and HIC plots.
