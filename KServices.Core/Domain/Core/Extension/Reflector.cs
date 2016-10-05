using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MedTeam.Infrastructure.Utils
{
    /// <summary>
    /// Represents strongly typed reflection.
    /// </summary>
    public static class Reflector
    {
        #region Delegates

        /// <summary>
        /// The delegate of parameter less operation.
        /// </summary>
        /// <returns>The operation result.</returns>
        public delegate object Operation();

        /// <summary>
        /// The delegate of parameter less operation.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <returns> The operation result.</returns>
        public delegate object Operation<in T>(T declaringType);

        /// <summary>
        /// The delegate of operation with 1 argument.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <typeparam name="TA0">The type of 0 argument.</typeparam>
        /// <param name="arg0">The 0 argument.</param>
        /// <returns>The operation result.</returns>
        public delegate object Operation<in T, in TA0>(T declaringType, TA0 arg0);

        /// <summary>
        /// The delegate of operation with 2 argument.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <typeparam name="TA0">The type of 0 argument.</typeparam>
        /// <param name="arg0">The 0 argument.</param>
        /// <typeparam name="TA1">The type of 1 argument.</typeparam>
        /// <param name="arg1">The 1 argument.</param>
        /// <returns>The operation result.</returns>
        public delegate object Operation<in T, in TA0, in TA1>(T declaringType, TA0 arg0, TA1 arg1);

        /// <summary>
        /// The delegate of operation with 3 argument.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <typeparam name="TA0">The type of 0 argument.</typeparam>
        /// <param name="arg0">The 0 argument.</param>
        /// <typeparam name="TA1">The type of 1 argument.</typeparam>
        /// <param name="arg1">The 1 argument.</param>
        /// <typeparam name="TA2">The type of 2 argument.</typeparam>
        /// <param name="arg2">The 2 argument.</param>
        /// <returns>The operation result.</returns>
        public delegate object Operation<in T, in TA0, in TA1, in TA2>(T declaringType, TA0 arg0, TA1 arg1, TA2 arg2);

        /// <summary>
        /// The delegate of operation with 4 argument.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <typeparam name="TA0">The type of 0 argument.</typeparam>
        /// <param name="arg0">The 0 argument.</param>
        /// <typeparam name="TA1">The type of 1 argument.</typeparam>
        /// <param name="arg1">The 1 argument.</param>
        /// <typeparam name="TA2">The type of 2 argument.</typeparam>
        /// <param name="arg2">The 2 argument.</param>
        /// <typeparam name="TA3">The type of 3 argument.</typeparam>
        /// <param name="arg3">The 3 argument.</param>
        /// <returns>The operation result.</returns>
        public delegate object Operation<in T, in TA0, in TA1, in TA2, in TA3>(
            T declaringType, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3);

        /// <summary>
        /// The delegate of operation with 5 argument.
        /// </summary>
        /// <typeparam name="T">The type of declaring class.</typeparam>
        /// <param name="declaringType">The declaring class.</param>
        /// <typeparam name="TA0">The type of 0 argument.</typeparam>
        /// <param name="arg0">The 0 argument.</param>
        /// <typeparam name="TA1">The type of 1 argument.</typeparam>
        /// <param name="arg1">The 1 argument.</param>
        /// <typeparam name="TA2">The type of 2 argument.</typeparam>
        /// <param name="arg2">The 2 argument.</param>
        /// <typeparam name="TA3">The type of 3 argument.</typeparam>
        /// <param name="arg3">The 3 argument.</param>
        /// <typeparam name="TA4">The type of 4 argument.</typeparam>
        /// <param name="arg4">The 4 argument.</param>
        /// <returns>The operation result.</returns>
        public delegate object Operation<in T, in TA0, in TA1, in TA2, in TA3, in TA4>(
            T declaringType, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3, TA4 arg4);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets field info by the specified field expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <param name="field">The field expression.</param>
        /// <returns>The field info.</returns>
        public static FieldInfo Field<TDeclaringType>(Expression<Operation<TDeclaringType>> field)
        {
            var info = GetMemberInfo(field) as FieldInfo;
            if (info == null)
            {
                throw new ArgumentException("Member is not a field");
            }

            return info;
        }

        /// <summary>
        /// Gets the all public properties of type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns>The all public properties from specified type.</returns>
        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return GetProperties(typeof(T));
        }

        /// <summary>
        /// Gets the all public properties of type.
        /// </summary>
        /// <param name="type">The specified type.</param>
        /// <returns>
        /// The all public properties from specified type.
        /// </returns>
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties();
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method(Expression<Operation> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType>(Expression<Operation<TDeclaringType>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <typeparam name="TA0">The type of the argument.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType, TA0>(Expression<Operation<TDeclaringType, TA0>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <typeparam name="TA0">The type of the 0 argument.</typeparam>
        /// <typeparam name="TA1">The type of the 1 argument.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType, TA0, TA1>(
            Expression<Operation<TDeclaringType, TA0, TA1>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <typeparam name="TA0">The type of the 0 argument.</typeparam>
        /// <typeparam name="TA1">The type of the 1 argument.</typeparam>
        /// <typeparam name="TA2">The type of the 2 argument.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType, TA0, TA1, TA2>(
            Expression<Operation<TDeclaringType, TA0, TA1, TA2>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <typeparam name="TA0">The type of the 0 argument.</typeparam>
        /// <typeparam name="TA1">The type of the 1 argument.</typeparam>
        /// <typeparam name="TA2">The type of the 2 argument.</typeparam>
        /// <typeparam name="TA3">The type of the 3 argument.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType, TA0, TA1, TA2, TA3>(
            Expression<Operation<TDeclaringType, TA0, TA1, TA2, TA3>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Get method info by the specified method expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <typeparam name="TA0">The type of the 0 argument.</typeparam>
        /// <typeparam name="TA1">The type of the 1 argument.</typeparam>
        /// <typeparam name="TA2">The type of the 2 argument.</typeparam>
        /// <typeparam name="TA3">The type of the 3 argument.</typeparam>
        /// <typeparam name="TA4">The type of the 4 argument.</typeparam>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo Method<TDeclaringType, TA0, TA1, TA2, TA3, TA4>(
            Expression<Operation<TDeclaringType, TA0, TA1, TA2, TA3, TA4>> method)
        {
            return GetMethodInfo(method);
        }

        /// <summary>
        /// Gets property info by the specified property expression.
        /// </summary>
        /// <typeparam name="TDeclaringType">The type of the declaring type.</typeparam>
        /// <param name="property">The property expression.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo Property<TDeclaringType>(Expression<Operation<TDeclaringType>> property)
        {
            var info = GetMemberInfo(property) as PropertyInfo;
            if (info == null)
            {
                throw new ArgumentException("Member is not a property");
            }

            return info;
        }

        #region Custom Attribute

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
    where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Gets the member info by expression.
        /// </summary>
        /// <param name="member">The member expression.</param>
        /// <returns>The member info.</returns>
        private static MemberInfo GetMemberInfo(Expression member)
        {
            var lambda = member as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentNullException("member");
            }

            MemberExpression memberExpr = null;

            // Our Operation<T> returns an object, so first statement can be either 
            // a cast (if method does not return an object) or the direct method call.
            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                // The cast is an unary expression, where the operand is the 
                // actual method call expression.
                memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else
            {
                if (lambda.Body.NodeType == ExpressionType.MemberAccess)
                {
                    memberExpr = lambda.Body as MemberExpression;
                }
            }

            if (memberExpr == null)
            {
                throw new ArgumentException("member");
            }

            return memberExpr.Member;
        }

        /// <summary>
        /// Gets the method info by method expression.
        /// </summary>
        /// <param name="method">The method expression.</param>
        /// <returns>The method info</returns>
        private static MethodInfo GetMethodInfo(Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentNullException("method");
            }

            MethodCallExpression methodExpr = null;

            // Our Operation<T> returns an object, so first statement can be either 
            // a cast (if method does not return an object) or the direct method call.
            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                // The cast is an unary expression, where the operand is the 
                // actual method call expression.
                methodExpr = ((UnaryExpression)lambda.Body).Operand as MethodCallExpression;
            }
            else
            {
                if (lambda.Body.NodeType == ExpressionType.Call)
                {
                    methodExpr = lambda.Body as MethodCallExpression;
                }
            }

            if (methodExpr == null)
            {
                throw new ArgumentException("method");
            }

            return methodExpr.Method;
        }

        #endregion
    }
}