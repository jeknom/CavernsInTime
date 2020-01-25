using System;


namespace Utils
{
    public class Option<T>
    {
        readonly bool hasValue;
        readonly T value;

        public Option(T value = default) {
            this.hasValue = value != null;
            this.value = value;
        }

        public void Match(Action<T> some, Action none)
        {
            if (this.hasValue)
            {
                some(this.value);
            }
            else
            {
                none();
            }
        }

        public void MatchSome(Action<T> f)
        {
            if (this.hasValue)
            {
                f(this.value);
            }
        }

        public void MatchNone(Action f)
        {
            if (!this.hasValue)
            {
                f();
            }
        }
    }
}
