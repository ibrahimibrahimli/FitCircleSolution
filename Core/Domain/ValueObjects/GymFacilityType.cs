    using System.Collections.ObjectModel;

    namespace Domain.ValueObjects
    {
        public class GymFacilityType : IEquatable<GymFacilityType>
        {
            public static readonly GymFacilityType Cardio = new(1, "Cardio", "Cardiovascular equipment area", "🏃‍♂️", true, 50);
            public static readonly GymFacilityType Strength = new(2, "Strength", "Weight training and resistance equipment", "💪", true, 30);
            public static readonly GymFacilityType Pool = new(3, "Pool", "Swimming pool facility", "🏊‍♂️", true, 25);
            public static readonly GymFacilityType Sauna = new(4, "Sauna", "Steam and relaxation area", "🧖‍♂️", false, 10);
            public static readonly GymFacilityType Yoga = new(5, "Yoga", "Yoga and meditation studio", "🧘‍♀️", false, 20);
            public static readonly GymFacilityType Pilates = new(6, "Pilates", "Pilates exercise studio", "🤸‍♀️", false, 15);
            public static readonly GymFacilityType CrossFit = new(7, "CrossFit", "High-intensity functional fitness", "🏋️‍♂️", true, 20);
            public static readonly GymFacilityType Boxing = new(8, "Boxing", "Boxing and martial arts area", "🥊", true, 15);
            public static readonly GymFacilityType Basketball = new(9, "Basketball", "Basketball court", "🏀", true, 10);
            public static readonly GymFacilityType Tennis = new(10, "Tennis", "Tennis court facility", "🎾", true, 4);
            public static readonly GymFacilityType Spinning = new(11, "Spinning", "Indoor cycling studio", "🚴‍♀️", true, 25);
            public static readonly GymFacilityType GroupFitness = new(12, "Group Fitness", "Group exercise classes", "👥", false, 30);
            public static readonly GymFacilityType PersonalTraining = new(13, "Personal Training", "One-on-one training area", "👨‍🏫", false, 2);
            public static readonly GymFacilityType Locker = new(14, "Locker Room", "Changing and storage facility", "🚿", false, 50);
            public static readonly GymFacilityType Cafe = new(15, "Cafe", "Food and beverage area", "☕", false, 20);

            private static readonly ReadOnlyCollection<GymFacilityType> _all = new List<GymFacilityType>
            {
                Cardio, Strength, Pool, Sauna, Yoga, Pilates, CrossFit,
                Boxing, Basketball, Tennis, Spinning, GroupFitness,
                PersonalTraining, Locker, Cafe
            }.AsReadOnly();

            public int Id { get; }
            public string Name { get; }
            public string Description { get; }
            public string Icon { get; }
            public bool RequiresEquipment { get; }
            public int DefaultMaxCapacity { get; }


            public GymFacilityType()
            {
            
            }
            private GymFacilityType(int id, string name, string description, string icon, bool requiresEquipment, int defaultMaxCapacity)
            {
                Id = id;
                Name = name;
                Description = description;
                Icon = icon;
                RequiresEquipment = requiresEquipment;
                DefaultMaxCapacity = defaultMaxCapacity;
            }

            // Static Methods
            public static IReadOnlyCollection<GymFacilityType> GetAll() => _all;

            public static GymFacilityType FromId(int id)
            {
                return _all.FirstOrDefault(x => x.Id == id)
                    ?? throw new ArgumentException($"Invalid GymFacilityType ID: {id}");
            }

            public static GymFacilityType FromName(string name)
            {
                return _all.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    ?? throw new ArgumentException($"Invalid GymFacilityType name: {name}");
            }

            public static bool TryFromId(int id, out GymFacilityType facilityType)
            {
                facilityType = _all.FirstOrDefault(x => x.Id == id);
                return facilityType != null;
            }

            public static bool TryFromName(string name, out GymFacilityType facilityType)
            {
                facilityType = _all.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return facilityType != null;
            }

            // Instance Methods
            public bool IsCompatibleWith(GymFacilityType other)
            {
                if (other == null) return false;
                if (this == other) return true;

                return (this, other) switch
                {
                    var (a, b) when (a == Cardio && b == Strength) || (a == Strength && b == Cardio) => true,
                    var (a, b) when (a == Pool && b == Sauna) || (a == Sauna && b == Pool) => true,
                    var (a, b) when (a == Yoga && b == Pilates) || (a == Pilates && b == Yoga) => true,
                    var (a, b) when (a == Boxing && b == CrossFit) || (a == CrossFit && b == Boxing) => true,
                    _ => false
                };
            }

            public bool RequiresSpecialCertification()
            {
                return this == Pool || this == PersonalTraining || this == CrossFit;
            }

            public TimeSpan GetRecommendedSessionDuration()
            {
                return this switch
                {
                    var t when t == Cardio => TimeSpan.FromMinutes(45),
                    var t when t == Strength => TimeSpan.FromMinutes(60),
                    var t when t == Pool => TimeSpan.FromMinutes(30),
                    var t when t == Sauna => TimeSpan.FromMinutes(20),
                    var t when t == Yoga => TimeSpan.FromMinutes(75),
                    var t when t == Pilates => TimeSpan.FromMinutes(60),
                    var t when t == CrossFit => TimeSpan.FromMinutes(60),
                    var t when t == Boxing => TimeSpan.FromMinutes(90),
                    var t when t == Spinning => TimeSpan.FromMinutes(45),
                    var t when t == GroupFitness => TimeSpan.FromMinutes(60),
                    var t when t == PersonalTraining => TimeSpan.FromMinutes(60),
                    _ => TimeSpan.FromMinutes(60)
                };
            }

            public decimal GetBaseHourlyRate()
            {
                return this switch
                {
                    var t when t == PersonalTraining => 80m,
                    var t when t == CrossFit => 25m,
                    var t when t == Boxing => 30m,
                    var t when t == Tennis => 40m,
                    var t when t == Pool => 20m,
                    _ => 15m
                };
            }

            // Equality Implementation
            public bool Equals(GymFacilityType other)
            {
                return other != null && Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as GymFacilityType);
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public static bool operator ==(GymFacilityType left, GymFacilityType right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(GymFacilityType left, GymFacilityType right)
            {
                return !Equals(left, right);
            }

            public override string ToString() => $"{Icon} {Name}";

            // Implicit conversion for convenience
            public static implicit operator int(GymFacilityType facilityType) => facilityType?.Id ?? 0;
            public static implicit operator string(GymFacilityType facilityType) => facilityType?.Name ?? string.Empty;
        }
    }