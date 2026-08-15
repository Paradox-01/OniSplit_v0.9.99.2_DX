namespace Oni
{
	internal delegate TResult Func<T1, TResult>(T1 arg1);
	internal delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
	internal delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
}
