using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PortalVet.Data.Helper
{
    public static class Enumeradores
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static string GetCategory(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    CategoryAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(CategoryAttribute)) as CategoryAttribute;
                    if (attr != null)
                    {
                        return attr.Category;
                    }
                }
            }
            return null;
        }
    }

    public enum EnumAdminProfile
    {
        NULL = 0,
        [Description("Administrador")]
        Administrador = 1,
        [Description("Gerente")]
        Gerente = 2,
        [Description("Clínica")]
        Clinica = 3,
        [Description("Cliente")]
        Cliente = 4,
        [Description("Laudador")]
        Laudador = 5
    }

    public enum EnumExameStatus
    {
        [Description("Aguardando atendimento")]
        Aguardando_Atendimento = 1,
        [Description("Em Andamento")]
        Em_Andamento = 2,
        [Description("Cancelado")]
        Cancelado = 3,
        [Description("Concluído")]
        Concluido = 4,
    }

    public enum EnumExameSituacao
    {
        [Description("Criação")]
        Criacao = 1,
        //[Description("Em Análise (Clínica)")]
        //Em_Analise_Clinica = 2,
        [Description("Em Análise (Laudador)")]
        Em_Analise_Laudador = 3,
        //[Description("Em Análise (Gerente)")]
        //Em_Analise_Gerente = 4,
        [Description("Em Análise (Gerente)")]
        Concluido_Gerente = 5,
        [Description("Concluído")]
        Concluido = 6,
    }


    public enum EnumExameSituacaoLaudador
    {
        [Description("Em Análise")]
        Em_Analise = 1,
        [Description("Concluído")]
        Concluido = 2,
        [Description("Pendente")]
        Pendente = 3,
    }
}
