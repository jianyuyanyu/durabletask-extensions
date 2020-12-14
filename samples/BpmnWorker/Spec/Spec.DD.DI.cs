//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.0.370.0.
namespace Spec.DD.DI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("DiagramElement", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DiagramElement", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNEdge))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNLabel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNPlane))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNShape))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Edge))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Label))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LabeledEdge))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LabeledShape))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Node))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Plane))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Shape))]
    public abstract partial class DiagramElement
    {
        
        [System.Xml.Serialization.XmlElementAttribute("extension")]
        public DiagramElementExtension Extension { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<System.Xml.XmlAttribute> _anyAttribute;
        
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Collections.ObjectModel.Collection<System.Xml.XmlAttribute> AnyAttribute
        {
            get
            {
                return this._anyAttribute;
            }
            private set
            {
                this._anyAttribute = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="DiagramElement" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="DiagramElement" /> class.</para>
        /// </summary>
        public DiagramElement()
        {
            this._anyAttribute = new System.Collections.ObjectModel.Collection<System.Xml.XmlAttribute>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("DiagramElementExtension", Namespace="http://www.omg.org/spec/DD/20100524/DI", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DiagramElementExtension
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<System.Xml.XmlElement> _any;
        
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Collections.ObjectModel.Collection<System.Xml.XmlElement> Any
        {
            get
            {
                return this._any;
            }
            private set
            {
                this._any = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Any-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Any collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AnySpecified
        {
            get
            {
                return (this.Any.Count != 0);
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="DiagramElementExtension" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="DiagramElementExtension" /> class.</para>
        /// </summary>
        public DiagramElementExtension()
        {
            this._any = new System.Collections.ObjectModel.Collection<System.Xml.XmlElement>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Diagram", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Diagram", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNDiagram))]
    public abstract partial class Diagram
    {
        
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("documentation")]
        public string Documentation { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlAttributeAttribute("resolution")]
        public double ResolutionValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Resolution-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Resolution property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ResolutionValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<double> Resolution
        {
            get
            {
                if (this.ResolutionValueSpecified)
                {
                    return this.ResolutionValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ResolutionValue = value.GetValueOrDefault();
                this.ResolutionValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Node", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Node", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNLabel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNPlane))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNShape))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Label))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LabeledShape))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Plane))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Shape))]
    public abstract partial class Node : DiagramElement
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Edge", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Edge", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNEdge))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LabeledEdge))]
    public abstract partial class Edge : DiagramElement
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Spec.DD.DC.Point> _waypoint;
        
        [System.Xml.Serialization.XmlElementAttribute("waypoint")]
        public System.Collections.ObjectModel.Collection<Spec.DD.DC.Point> Waypoint
        {
            get
            {
                return this._waypoint;
            }
            private set
            {
                this._waypoint = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Edge" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Edge" /> class.</para>
        /// </summary>
        public Edge()
        {
            this._waypoint = new System.Collections.ObjectModel.Collection<Spec.DD.DC.Point>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("LabeledEdge", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LabeledEdge", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNEdge))]
    public abstract partial class LabeledEdge : Edge
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Shape", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Shape", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNShape))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LabeledShape))]
    public abstract partial class Shape : Node
    {
        
        [System.Xml.Serialization.XmlElementAttribute("Bounds", Namespace="http://www.omg.org/spec/DD/20100524/DC")]
        public Spec.DD.DC.Bounds Bounds { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("LabeledShape", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LabeledShape", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNShape))]
    public abstract partial class LabeledShape : Shape
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Label", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Label", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNLabel))]
    public abstract partial class Label : Node
    {
        
        [System.Xml.Serialization.XmlElementAttribute("Bounds", Namespace="http://www.omg.org/spec/DD/20100524/DC")]
        public Spec.DD.DC.Bounds Bounds { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Plane", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Plane", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNPlane))]
    public abstract partial class Plane : Node
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<DiagramElement> _diagramElement;
        
        [System.Xml.Serialization.XmlElementAttribute("BPMNShape", Type=typeof(Spec.BPMN.DI.BPMNShape), Namespace="http://www.omg.org/spec/BPMN/20100524/DI")]
        [System.Xml.Serialization.XmlElementAttribute("BPMNEdge", Type=typeof(Spec.BPMN.DI.BPMNEdge), Namespace="http://www.omg.org/spec/BPMN/20100524/DI")]
        [System.Xml.Serialization.XmlElementAttribute("DiagramElement")]
        public System.Collections.ObjectModel.Collection<DiagramElement> DiagramElement
        {
            get
            {
                return this._diagramElement;
            }
            private set
            {
                this._diagramElement = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DiagramElement-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the DiagramElement collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiagramElementSpecified
        {
            get
            {
                return (this.DiagramElement.Count != 0);
            }
        }
        
        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Plane" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Plane" /> class.</para>
        /// </summary>
        public Plane()
        {
            this._diagramElement = new System.Collections.ObjectModel.Collection<DiagramElement>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.370.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("Style", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Style", Namespace="http://www.omg.org/spec/DD/20100524/DI")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Spec.BPMN.DI.BPMNLabelStyle))]
    public abstract partial class Style
    {
        
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }
    }
}
