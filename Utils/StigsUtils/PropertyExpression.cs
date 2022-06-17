// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Linq.Expressions;
using System.Reflection;
namespace StigsUtils;

/// <summary>
///   Utility class to make it easy to extract an Action and a Func to set and get the property pinpointed by the
///   getExpression parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TProp"></typeparam>
public sealed class PropertyExpression<T, TProp> {
	public PropertyExpression(Expression<Func<T, TProp>> getExpression) {
		var memberExpression = (MemberExpression)getExpression.Body;
		var prop = (PropertyInfo)memberExpression.Member;
		MethodInfo? setMethod = prop.GetSetMethod();
		ParameterExpression parameterT = Expression.Parameter(typeof(T), "x");
		ParameterExpression parameterTProperty = Expression.Parameter(typeof(TProp), "y");
		Expression<Action<T, TProp>> setExpression =
			Expression.Lambda<Action<T, TProp>>(
				Expression.Call(parameterT, setMethod!, parameterTProperty),
				parameterT,
				parameterTProperty
			);
		Getter = getExpression.Compile();
		Setter = setExpression.Compile();
	}

	public Func<T, TProp> Getter { get; set; }
	public Action<T, TProp> Setter { get; set; }
}