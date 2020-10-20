using System.Linq;
using System.Text;

namespace CLS.Core.Data
{
    public partial class AlertTriggerGroup
    {
        public string ExpressionString
        {
            get
            {
                var nodes = AlertTriggerNodes.OrderBy(x => x.PositionInGroup).ToList();
                var expression = new StringBuilder();
                foreach (var node in nodes)
                {
                    switch (node.AlertTriggerNodeOperator?.AlertTriggerNodeType.Name)
                    {
                        case "VariableName":
                        {
                            expression.Append(node.AlertTriggerNodeOperator.DotNetProperty);
                            break;
                        }
                        case "ComparisonOperator":
                        case "LogicalOperator":
                        {
                            expression.Append(node.AlertTriggerNodeOperator.Value);
                            break;
                        }
                        case "DynamicVariable":
                        {
                            expression.Append(node.DynamicNodeValue);
                            break;
                        }
                        default:
                        {
                            expression.Append("\"" + node.DynamicNodeValue + "\"");
                            break;
                        }
                    }
                }
                return expression.ToString();
            }
        }
    }
}
