namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
internal class MersenneTwister
{
    #region Fields

    private const short _n = 624;
    private const short _m = 397;
    private const uint _matrixA = 0x9908b0df;
    private const uint _upperMask = 0x80000000;
    private const uint _lowerMask = 0x7fffffff;
    private uint[]? _mt;
    private ushort _mti;
    private uint[]? _mag01;
    private bool _disposed;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MersenneTwister"/> class with given seed value.
    /// </summary>
    /// <param name="seed">has to send a seed.  because in testing, had duplicate random numbers.</param>
    public MersenneTwister(uint seed)
    {
        Init();
        InitGenRand(seed);
    }
    ~MersenneTwister()
    {
        Dispose(false);
    }
    #endregion

    #region Methods

    /// <summary>
    /// Releases all resources used by the current instance of <see cref="MersenneTwister"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the current instance of <see cref="MersenneTwister"/>.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _mt = null!;
            _mag01 = null!;
        }

        _disposed = true;
    }

    private void Init()
    {
        _mt = new uint[_n];
        _mag01 = new uint[] { 0, _matrixA };
        _mti = _n + 1;
    }

    /// <summary>
    /// Initializes mt[N] with a seed.
    /// </summary>
    /// <param name="seed">Seed value.</param>
    private void InitGenRand(uint seed)
    {
        _mt![0] = seed;
        for (_mti = 1; _mti < _n; _mti++)
            _mt[_mti] = 1812433253 * (_mt[_mti - 1] ^ _mt[_mti - 1] >> 30) + _mti;
    }

    public uint GenRandInt32()
    {
        uint y;

        if (_mti >= _n)
        {
            short kk;

            if (_mti == _n + 1)
            {
                InitGenRand(5489);
            }

            for (kk = 0; kk < _n - _m; kk++)
            {
                y = (_mt![kk] & _upperMask | _mt[kk + 1] & _lowerMask) >> 1;
                _mt[kk] = _mt[kk + _m] ^ _mag01![_mt[kk + 1] & 1] ^ y;
            }

            for (; kk < _n - 1; kk++)
            {
                y = (_mt![kk] & _upperMask | _mt[kk + 1] & _lowerMask) >> 1;
                _mt[kk] = _mt[kk + (_m - _n)] ^ _mag01![_mt[kk + 1] & 1] ^ y;
            }

            y = (_mt![_n - 1] & _upperMask | _mt[0] & _lowerMask) >> 1;
            _mt[_n - 1] = _mt[_m - 1] ^ _mag01![_mt[0] & 1] ^ y;
            _mti = 0;
        }
        y = _mt![_mti++];
        y ^= y >> 11;
        y ^= y << 7 & 0x9d2c5680;
        y ^= y << 15 & 0xefc60000;
        y ^= y >> 18;

        return y;
    }

    /// <summary>
    /// Generates a random number on [0,0x7fffffff]-Interval.
    /// </summary>
    /// <returns>Returns generated number.</returns>
    public uint GenRandInt31() => GenRandInt32() >> 1;

    /// <summary>
    /// Generates a random number on [0,1]-real-Interval.
    /// </summary>
    /// <returns>Returns generated number.</returns>
    public double GenRandReal1() => GenRandInt32() * (1.0 / 4294967295.0);

    /// <summary>
    /// Generates a random number on [0,1)-real-Interval.
    /// </summary>
    /// <returns>Returns generated number.</returns>
    public double GenRandReal2() => GenRandInt32() * (1.0 / 4294967296.0);

    /// <summary>
    /// Generates a random number on (0,1)-real-Interval
    /// </summary>
    /// <returns>Returns generated number.</returns>
    public double GenRandReal3() => (GenRandInt32() + 0.5) * (1.0 / 4294967296.0);

    /// <summary>
    /// Generates a random number on [0,1) with 53-bit resolution.
    /// </summary>
    /// <returns>Returns generated number.</returns>
    public double GenRandRes53()
    {
        uint a = GenRandInt32() >> 5, b = GenRandInt32() >> 6;

        return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
    }

    #endregion
}