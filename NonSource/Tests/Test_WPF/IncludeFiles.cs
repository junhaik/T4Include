﻿
// ############################################################################
// #                                                                          #
// #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
// #                                                                          #
// # This means that any edits to the .cs file will be lost when its          #
// # regenerated. Changes should instead be applied to the corresponding      #
// # text template file (.tt)                                                      #
// ############################################################################



// ############################################################################
// @@@ INCLUDING: C:\temp\GitHub\T4Include\WPF\AnimatedEntrance.cs
// @@@ INCLUDE_FOUND: Generated_AnimatedEntrance_DependencyProperties.cs
// @@@ INCLUDE_FOUND: Generated_AnimatedEntrance_StateMachine.cs
// @@@ INCLUDE_FOUND: ../Extensions/WpfExtensions.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\WPF\AccordionPanel.cs
// @@@ INCLUDE_FOUND: Generated_AccordionPanel_DependencyProperties.cs
// @@@ INCLUDE_FOUND: ../Extensions/WpfExtensions.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_DependencyProperties.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_StateMachine.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\Extensions\WpfExtensions.cs
// @@@ INCLUDE_FOUND: ../Common/Array.cs
// @@@ INCLUDE_FOUND: ../Common/Log.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\WPF\Generated_AccordionPanel_DependencyProperties.cs
// @@@ SKIPPING (Already seen): C:\temp\GitHub\T4Include\Extensions\WpfExtensions.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\Common\Array.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\Common\Log.cs
// @@@ INCLUDE_FOUND: Config.cs
// @@@ INCLUDE_FOUND: Generated_Log.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\Common\Config.cs
// @@@ INCLUDING: C:\temp\GitHub\T4Include\Common\Generated_Log.cs
// ############################################################################
// Certains directives such as #define and // Resharper comments has to be 
// moved to top in order to work properly    
// ############################################################################
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable InconsistentNaming
// ReSharper disable InvocationIsSkipped
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBeProtected.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PossibleUnintendedReferenceComparison
// ReSharper disable RedundantAssignment
// ReSharper disable RedundantCaseLabel
// ReSharper disable RedundantCast
// ReSharper disable RedundantIfElseBlock
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable UnassignedField.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\WPF\AnimatedEntrance.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    
    
    
    namespace Source.WPF
    {
        using Source.Extensions;
        using System;
        using System.Collections.ObjectModel;
        using System.Linq;
        using System.Windows;
        using System.Windows.Controls;
        using System.Windows.Markup;
        using System.Windows.Media;
        using System.Windows.Media.Animation;
        using System.Windows.Threading;
    
        [TemplatePart(Name = PART_Panel    , Type = typeof(Panel))]
        [ContentProperty("Children")]
        partial class AnimatedEntrance : Control
        {
            const string PART_Panel    = @"PART_Panel"    ;
        
            public enum Option
            {
                Instant         ,
                Fade            ,
                PushFromLeft    ,
                PushFromRight   ,
                PushFromTop     ,
                PushFromBottom  ,
                CoverFromLeft   ,
                CoverFromRight  ,
                CoverFromTop    ,
                CoverFromBottom ,
                RevealToLeft    ,
                RevealToRight   ,
                RevealToTop     , 
                RevealToBottom  ,
            }
        
            sealed partial class PendingRequest
            {
                public Option    PresentOption  ;
                public UIElement Next           ;
            }
    
            abstract partial class BaseState
            {
                public AnimatedEntrance Owner       ;
                public PendingRequest   Request     ;
    
                partial void Initialize_BaseState(BaseState state)
                {
                    state.Owner     = Owner     ;
                    state.Request   = Request   ;
                }
    
            }
    
            partial class State_PresentingContent
            {
                public  UIElement   Current     ;
    
                partial void IsEdgeValid_PresentingContent_To_Transitioning(Option presentOption, UIElement next, ref bool result)
                {
                    result = 
                            !ReferenceEquals(Current, next)
                        &&  (next == null || Owner.Children.Contains(next))
                        ;
                }
    
                partial void TransformInto_Transitioning(Option presentOption, UIElement next, State_Transitioning nextState)
                {
                    nextState.Previous      = Current       ;
                    nextState.Next          = next          ;
                    nextState.PresentOption = presentOption ;
                }
                
            }
    
            partial class State_Transitioning
            {
                public UIElement        Previous                ;
                public UIElement        Next                    ;
                public Option           PresentOption           ;
                public AnimationClock   Clock                   ;
    
                public readonly Vector  PreviousOffset_Start    = new Vector ();
                public Vector           PreviousOffset_End      ;
                public Vector           NextOffset_Start        ;
                public readonly Vector  NextOffset_End          = new Vector ();
    
                public TranslateTransform   PreviousTransform   ;
                public TranslateTransform   NextTransform       ;
    
                Vector GetOffset()
                {
                    switch (PresentOption)
                    {
                        case Option.PushFromLeft:
                        case Option.CoverFromLeft:
                        case Option.RevealToLeft:
                            return new Vector (-Owner.ActualWidth, 0);
                        case Option.PushFromRight:
                        case Option.CoverFromRight:
                        case Option.RevealToRight:
                            return new Vector (Owner.ActualWidth, 0);
                        case Option.PushFromTop:
                        case Option.CoverFromTop:
                        case Option.RevealToTop:
                            return new Vector (0, -Owner.ActualHeight);
                        case Option.PushFromBottom:
                        case Option.CoverFromBottom:
                        case Option.RevealToBottom:
                            return new Vector (0, Owner.ActualHeight);
                        default:
                        case Option.Instant:
                        case Option.Fade:
                            return new Vector ();
                    }
                }
    
                partial void Enter_Transitioning(BaseState previousState)
                {
                    var nextOpacity = 1.0;
                    switch (PresentOption)
                    {
                        case Option.Fade:
                            nextOpacity = 0;                      
                            break;
                        case Option.PushFromLeft:
                        case Option.PushFromRight:
                        case Option.PushFromTop:
                        case Option.PushFromBottom:
                            NextOffset_Start        = GetOffset();
                            PreviousOffset_End      = -NextOffset_Start;
                            break;
                        case Option.CoverFromLeft:
                        case Option.CoverFromRight:
                        case Option.CoverFromTop:
                        case Option.CoverFromBottom:
                            NextOffset_Start        = GetOffset();
                            break;
                        case Option.RevealToLeft:
                        case Option.RevealToRight:
                        case Option.RevealToTop:
                        case Option.RevealToBottom:
                            PreviousOffset_End      = -GetOffset();
                            break;
                        default:
                        case Option.Instant:
                            break;
                    }
    
                    PreviousTransform               = PreviousOffset_Start.ToTranslateTransform();
                    NextTransform                   = NextOffset_Start.ToTranslateTransform();
    
                    Owner.m_current.Opacity         = 1                     ;
                    Owner.m_current.RenderTransform = PreviousTransform     ;
    
                    Owner.m_next.Opacity            = nextOpacity           ;
                    Owner.m_next.RenderTransform    = NextTransform         ;
    
                    Owner.m_next.Child  = Next;
    
                    switch (PresentOption)
                    {
                        case Option.RevealToLeft:
                        case Option.RevealToRight:
                        case Option.RevealToTop:
                        case Option.RevealToBottom:
                            Owner.m_panel.Children.Insert(0, Owner.m_next);
                            break;
                        case Option.Instant:
                        case Option.Fade:
                        case Option.PushFromLeft:
                        case Option.PushFromRight:
                        case Option.PushFromTop:
                        case Option.PushFromBottom:
                        case Option.CoverFromLeft:
                        case Option.CoverFromRight:
                        case Option.CoverFromTop:
                        case Option.CoverFromBottom:
                        default:
                            Owner.m_panel.Children.Add(Owner.m_next);
                            break;
                    }
    
                    if (PresentOption != Option.Instant)
                    {
                        Owner.Dispatcher.Async_Invoke (
                            "AnimatedEntrance: Awaiting measure/arrange before starting animation", 
                            StartAnimation
                            );
                    }
                    else
                    {
                        Owner.Dispatcher.Async_Invoke(
                            "AnimatedEntrance: Switching to next control instantly", 
                            ShowInstant
                            );
                    }
    
                }
    
                void ShowInstant()
                {
                    FollowEdge();
                }
    
                void StartAnimation()
                {
                    Clock             =  s_transitionClock.CreateClock();
                    Clock.Completed   += Transition_Completed;
    
                    Owner.ApplyAnimationClock(AnimationClockProperty, Clock, HandoffBehavior.SnapshotAndReplace);
                }
    
                void Transition_Completed(object sender, EventArgs e)
                {
                    FollowEdge();
                }
    
                void FollowEdge()
                {
                    if (Request != null)
                    {
                        Owner.SetState(
                            this,
                            EdgeFrom_Transitioning_To_DelayingNextTransition(Next));
                    }
                    else
                    {
                        Owner.SetState(this, EdgeFrom_Transitioning_To_PresentingContent(Next));
                    }
                }
    
                partial void Leave_Transitioning(BaseState nextState)
                {
                    if (Clock != null)
                    {
                        Clock.Completed   -= Transition_Completed;
                    }
                    Owner.ApplyAnimationClock(AnimationClockProperty, null);
    
                    var tmp         = Owner.m_current;
                    Owner.m_current = Owner.m_next;
                    Owner.m_next    = tmp;
    
                    Owner.m_panel.Children.Remove(Owner.m_next);
    
                    Owner.m_current.Opacity             = 1     ;
                    Owner.m_current.RenderTransform     = null  ;
    
                    Owner.m_next.Opacity                = 1     ;
                    Owner.m_current.RenderTransform     = null  ;
                    
                    Owner.m_next.Child                  = null  ;
    
                }
    
                partial void TransformInto_PresentingContent(UIElement current, State_PresentingContent nextState)
                {
                    nextState.Current = current;
                }
    
                partial void TransformInto_DelayingNextTransition(UIElement current, State_DelayingNextTransition nextState)
                {
                    nextState.Current = current;
                }
    
                public void Tick()
                {
                    var clock = GetAnimationClock(Owner);
    
                    switch (PresentOption)
                    {
                        case Option.Fade:
                            Owner.m_next.Opacity    = clock.Interpolate (0.0, 1.0);
                            break;
                        case Option.PushFromLeft:
                        case Option.PushFromRight:
                        case Option.PushFromTop:
                        case Option.PushFromBottom:
                            PreviousTransform.UpdateFromVector(clock.Interpolate (PreviousOffset_Start, PreviousOffset_End));
                            NextTransform.UpdateFromVector(clock.Interpolate (NextOffset_Start, NextOffset_End));
                            break;
                        case Option.CoverFromLeft:
                        case Option.CoverFromRight:
                        case Option.CoverFromTop:
                        case Option.CoverFromBottom:
                            NextTransform.UpdateFromVector(clock.Interpolate (NextOffset_Start, NextOffset_End));
                            break;
                        case Option.RevealToLeft:
                        case Option.RevealToRight:
                        case Option.RevealToTop:
                        case Option.RevealToBottom:
                            PreviousTransform.UpdateFromVector(clock.Interpolate (PreviousOffset_Start, PreviousOffset_End));
                            break;
                        default:
                        case Option.Instant:
                            break;
                    }
                }
            }
    
            partial class State_DelayingNextTransition
            {
                public  UIElement   Current     ;
    
                partial void IsEdgeValid_DelayingNextTransition_To_Transitioning(Option presentOption, UIElement next, ref bool result)
                {
                    result = 
                            !ReferenceEquals(Current, next)
                        &&  (next == null || Owner.Children.Contains(next))
                        ;
                }
    
                partial void TransformInto_PresentingContent(UIElement current, State_PresentingContent nextState)
                {
                    nextState.Current = current;
                }
    
                partial void TransformInto_Transitioning(Option presentOption, UIElement next, State_Transitioning nextState)
                {
                    nextState.Previous      = Current       ;
                    nextState.Next          = next          ;
                    nextState.PresentOption = presentOption ;
                }
    
                partial void Enter_DelayingNextTransition(BaseState previousState)
                {
                    Owner.m_delay.Start ();
                }
    
                partial void Leave_DelayingNextTransition(BaseState nextState)
                {
                    Owner.m_delay.Stop ();
                }
                
            }
    
            sealed partial class State_Initial
            {
                partial void TransformInto_PresentingContent(UIElement current, State_PresentingContent nextState)
                {
                    nextState.Current = current;
                    nextState.Owner.m_current.Child = current;
                }
            }
    
            sealed partial class PresentVisitor : BaseStateVisitor
            {
                public readonly Option      PresentOption  ;
                public readonly UIElement   Next           ;
    
                public PresentVisitor(Option presentOption, UIElement next)
                {
                    PresentOption   = presentOption    ;
                    Next            = next      ;
                }
    
                public override BaseState Visit_Initial(State_Initial state)
                {
                    state.Request = new PendingRequest
                                        {
                                            PresentOption   = PresentOption ,
                                            Next            = Next          ,
                                        };
                    return state;
                }
    
                public override BaseState Visit_PresentingContent(State_PresentingContent state)
                {
                    state.Request = null;
                    return state.EdgeFrom_PresentingContent_To_Transitioning(
                            PresentOption     ,
                            Next              
                            );
                }
    
                public override BaseState Visit_Transitioning(State_Transitioning state)
                {
                    state.Request = new PendingRequest
                                        {
                                            PresentOption   = PresentOption ,
                                            Next            = Next          ,
                                        };
                    return state;
                }
    
                public override BaseState Visit_DelayingNextTransition(State_DelayingNextTransition state)
                {
                    state.Request = new PendingRequest
                                        {
                                            PresentOption   = PresentOption ,
                                            Next            = Next          ,
                                        };
                    return state;
                }
            }
    
    
            sealed partial class AnimationClockTickVisitor : BaseNoActionStateVisitor
            {
                public override BaseState Visit_Transitioning(State_Transitioning state)
                {
                    state.Tick();
                    return state;
                }
            }
    
            sealed partial class DelayVisitor : BaseNoActionStateVisitor
            {
                public override BaseState Visit_DelayingNextTransition(State_DelayingNextTransition state)
                {
                    var request = state.Request;
                    if (request != null && state.IsEdgeValid_DelayingNextTransition_To_Transitioning(request.PresentOption, request.Next))
                    {
                        return state.EdgeFrom_DelayingNextTransition_To_Transitioning (request.PresentOption, request.Next);
                    }
                    else
                    {
                        return state.EdgeFrom_DelayingNextTransition_To_PresentingContent (state.Current);
                    }
                }
            }
    
            sealed partial class InitializeVisitor : BaseThrowingStateVisitor
            {
                public override BaseState Visit_Initial(State_Initial state)
                {
                    var request = state.Request;
                    var firstChild = state.Owner.Children.FirstOrDefault (c => c != null);
                    if (request != null && request.Next != null)
                    {
                        return state.EdgeFrom_Initial_To_PresentingContent(request.Next);
                    }
                    else if (firstChild != null)
                    {
                        return state.EdgeFrom_Initial_To_PresentingContent(firstChild);
                    }
                    else
                    {
                        return state.EdgeFrom_Initial_To_PresentingContent(null);
                    }
                }
            }
    
            public const string DefaultStyle = @"
    <Style 
        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        TargetType=""{x:Type i:AnimatedEntrance}""
        >
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""{x:Type i:AnimatedEntrance}"">
                    <Grid x:Name=""PART_Panel"">
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    ";
        
            readonly static Style           s_defaultStyle      ;
            readonly static Duration        s_transitionDuration;
            readonly static Duration        s_delayDuration     ;
            readonly static DoubleAnimation s_transitionClock   ;
    
            static readonly AnimationClockTickVisitor s_animationClockTickVisitor = new AnimationClockTickVisitor();
            static readonly DelayVisitor s_delayVisitor = new DelayVisitor();
            static readonly InitializeVisitor s_initializeVisitor = new InitializeVisitor();
    
            DispatcherTimer                 m_delay             ;
            Panel                           m_panel;
            Border                          m_current   = new Border
                                                              {
                                                                  HorizontalAlignment = HorizontalAlignment.Stretch,
                                                                  VerticalAlignment = VerticalAlignment.Stretch,
                                                              };
            Border                          m_next      = new Border
                                                              {
                                                                  HorizontalAlignment = HorizontalAlignment.Stretch,
                                                                  VerticalAlignment = VerticalAlignment.Stretch,
                                                              };
    
            static AnimatedEntrance()
            {
                var parserContext = new ParserContext
                                {
                                    XamlTypeMapper = new XamlTypeMapper(new string[0])
                                };
            
                var type = typeof (AnimatedEntrance);
                var namespaceName = type.Namespace ?? "";
                var assemblyName = type.Assembly.FullName;
                parserContext.XamlTypeMapper.AddMappingProcessingInstruction("Internal", namespaceName, assemblyName);
                parserContext.XmlnsDictionary.Add("i", "Internal");
        
                s_defaultStyle = (Style)XamlReader.Parse(
                    DefaultStyle,
                    parserContext
                    );
        
                s_transitionDuration    = new Duration (TimeSpan.FromMilliseconds(400));
                s_delayDuration         = new Duration (TimeSpan.FromMilliseconds(200));
                s_transitionClock       = new DoubleAnimation(
                    0,
                    1,
                    s_transitionDuration, 
                    FillBehavior.Stop
                    );
                s_transitionClock.EasingFunction = new ExponentialEase
                                                       {
                                                           EasingMode = EasingMode.EaseInOut,
                                                       };
    
                StyleProperty.OverrideMetadata(typeof(AnimatedEntrance), new FrameworkPropertyMetadata(s_defaultStyle));
            }
        
            partial void Constructed__AnimatedEntrance()
            {
                m_delay = new DispatcherTimer (
                    s_delayDuration.TimeSpan,
                    DispatcherPriority.ApplicationIdle,
                    OnDelay,
                    Dispatcher
                    );
                m_delay.Stop ();
    
                m_currentState = new State_Initial
                                     {
                                         Owner = this,
                                     };
                Children = new ObservableCollection<UIElement> ();
            }
    
            partial void Changed_Children(ObservableCollection<UIElement> oldValue, ObservableCollection<UIElement> newValue)
            {
                // TODO: Handle the situation when a displayed element is removed
            }
    
            void OnDelay(object sender, EventArgs e)
            {
                m_delay.Stop ();
    
                UpdateState(s_delayVisitor);
    
            }
    
            protected override Size MeasureOverride(Size constraint)
            {
                m_panel.Measure(constraint);
                return m_current.DesiredSize;
            }
    
    
    
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                m_panel    = GetTemplateChild(PART_Panel) as Panel;
    
                if (m_panel != null)
                {
                    m_panel.Children.Add(m_current);
                    UpdateState (s_initializeVisitor);
                }
    
            }
    
            static partial void Changed_AnimationClock(DependencyObject dependencyObject, double oldValue, double newValue)
            {
                var animatedEntrance = dependencyObject as AnimatedEntrance;
                if (animatedEntrance == null)
                {
                    return;
                }
    
                animatedEntrance.UpdateState(s_animationClockTickVisitor);
            }
    
            public void Present (Option option, UIElement element)
            {
                UpdateState(new PresentVisitor(option, element));
            }
        
        }
    
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\WPF\AnimatedEntrance.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\WPF\AccordionPanel.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    
    
    
    namespace Source.WPF
    {
        using Source.Extensions;
        using System;
        using System.Windows;
        using System.Windows.Controls;
        using System.Windows.Media;
        using System.Windows.Media.Animation;
        using System.Windows.Input;
    
        partial class AccordionPanel : Panel
        {
            readonly static Duration        s_transitionDuration;
            readonly static DoubleAnimation s_transitionClock   ;
    
            static AccordionPanel ()
            {
                s_transitionDuration    = new Duration (TimeSpan.FromMilliseconds(400));
                s_transitionClock       = new DoubleAnimation(
                    0,
                    1,
                    s_transitionDuration, 
                    FillBehavior.Stop
                    );
    
                s_transitionClock.EasingFunction = new ExponentialEase
                                                       {
                                                           EasingMode = EasingMode.EaseInOut,
                                                       };
                
            }
    
            partial void Constructed__AccordionPanel()
            {
                MouseButtonEventHandler mouseButtonEventHandler = Mouse_Down;
                AddHandler (MouseDownEvent, mouseButtonEventHandler, handledEventsToo: true);
                ClipToBounds = true;
            }
    
            public sealed partial class State
            {
                public double               From        ;
                public double               To          ;
                public TranslateTransform   Transform   ;
                public double               GetCurrent (double clock)
                {
                    return clock.Interpolate(From, To);
                }
    
                public void Update(double x)
                {
                    if (Transform != null)
                    {
                        Transform.X = x;
                    }
                }
            }
    
            AnimationClock m_clock;
    
            protected override Size MeasureOverride(Size availableSize)
            {
                var count = Children.Count; 
                if (count == 0)
                {
                    return availableSize;
                }
    
                var previewWidth = PreviewWidth;
                var adjustedSize = new Size (
                    Math.Max (availableSize.Width - (count - 1) * previewWidth, previewWidth), 
                    availableSize.Height
                    );
    
                for (int index = 0; index < count; index++)
                {
                    var child = Children[index];
                    child.Measure(adjustedSize);
                }
    
                return availableSize;
            }
    
            protected override Size ArrangeOverride(Size finalSize)
            {
                var animationClock = GetAnimationClock (this);
    
                StopClock();
    
                var count = Children.Count; 
                if (count == 0)
                {
                    return finalSize;
                }
    
                var previewWidth = PreviewWidth;
                var adjustedSize = new Size (
                    Math.Max (finalSize.Width - (count - 1) * previewWidth, previewWidth), 
                    finalSize.Height
                    );
                var adjustedRect = adjustedSize.ToRect();
    
                var activeElement = ActiveElement;
    
                var desiredX = 0.0;
    
                var doAnimate = false;
    
                for (int index = 0; index < count; index++)
                {
                    var child = Children[index];
    
                    var state = GetChildState(child);
                    if (state == null)
                    {
                        state = new State
                                    {
                                        Transform   = new TranslateTransform(),
                                        From        = finalSize.Width, 
                                        To          = finalSize.Width, 
                                    };
                        SetChildState (child, state);
                    }
    
                    child.Arrange(adjustedRect);
                    child.RenderTransform = state.Transform;
    
                    var current = state.GetCurrent (animationClock); 
    
                    state.From  = current;
                    state.To    = desiredX;
    
                    state.Update (current);
    
                    doAnimate |= !state.From.IsNear (state.To);
    
                    if (ReferenceEquals (child, activeElement))
                    {
                        desiredX += adjustedSize.Width;
                    }
                    else
                    {
                        desiredX += previewWidth;
                    }
                }
    
                if (doAnimate)
                {
                    StartClock();
                }
    
                return finalSize;
            }
    
            void StartClock()
            {
                StopClock ();
                m_clock = s_transitionClock.CreateClock();
                m_clock.Completed += Transition_Completed;
                ApplyAnimationClock(AnimationClockProperty, m_clock, HandoffBehavior.SnapshotAndReplace);
            }
    
            void StopClock()
            {
                if (m_clock != null)
                {
                    m_clock.Completed -= Transition_Completed;
                    m_clock = null;
                    ApplyAnimationClock(AnimationClockProperty, null);
                }
    
            }
    
            void Transition_Completed(object sender, EventArgs e)
            {
                StopClock();
                SetAnimationClock(this, 1);
            }
    
            static partial void Changed_AnimationClock(DependencyObject dependencyObject, double oldValue, double newValue)
            {
                var accordionPanel = dependencyObject as AccordionPanel;
                if (accordionPanel == null)
                {
                    return;
                }
    
                var count = accordionPanel.Children.Count;
                for (int index = 0; index < count; index++)
                {
                    var child = accordionPanel.Children[index];
                    var state = GetChildState(child);
                    if (state != null)
                    {
                        state.Update (state.GetCurrent(newValue));
                    }
                    else
                    {
                        accordionPanel.InvalidateArrange();
                    }
                }
            }
    
            partial void Changed_ActiveElement(UIElement oldValue, UIElement newValue)
            {
                InvalidateArrange();
            }
    
            void Mouse_Down(object sender, MouseButtonEventArgs e)
            {
                var pos = e.GetPosition (this);
    
                var animationClock = GetAnimationClock (this);
    
                UIElement hit = null;
                var count = Children.Count;
                for (int index = 0; index < count; index++)
                {
                    var child = Children[index];
                    var state = GetChildState(child);
                    var current = state.GetCurrent(animationClock);
    
                    if (current > pos.X)
                    {
                        ActiveElement = hit;
                        return;
                    }
    
                    hit = child;
                }
    
                ActiveElement = hit;
            }
    
            partial void Coerce_PreviewWidth(double value, ref double coercedValue)
            {
                coercedValue = Math.Max(8,value);
            }
    
            partial void Changed_PreviewWidth(double oldValue, double newValue)
            {
                InvalidateArrange ();
            }
        }
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\WPF\AccordionPanel.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_DependencyProperties.cs
namespace FileInclude
{
    
    // ############################################################################
    // #                                                                          #
    // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
    // #                                                                          #
    // # This means that any edits to the .cs file will be lost when its          #
    // # regenerated. Changes should instead be applied to the corresponding      #
    // # template file (.tt)                                                      #
    // ############################################################################
    
    
    
                                       
    
    
    namespace Source.WPF
    {
        using System.Collections;
        using System.Collections.ObjectModel;
        using System.Collections.Specialized;
    
        using System.Windows;
        using System.Windows.Media;
    
        // ------------------------------------------------------------------------
        // AnimatedEntrance
        // ------------------------------------------------------------------------
        partial class AnimatedEntrance
        {
            #region Uninteresting generated code
            static readonly DependencyPropertyKey ChildrenPropertyKey = DependencyProperty.RegisterReadOnly (
                "Children",
                typeof (ObservableCollection<UIElement>),
                typeof (AnimatedEntrance),
                new FrameworkPropertyMetadata (
                    null,
                    FrameworkPropertyMetadataOptions.None,
                    Changed_Children,
                    Coerce_Children          
                ));
    
            public static readonly DependencyProperty ChildrenProperty = ChildrenPropertyKey.DependencyProperty;
    
            static void Changed_Children (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                var instance = dependencyObject as AnimatedEntrance;
                if (instance != null)
                {
                    var oldValue = (ObservableCollection<UIElement>)eventArgs.OldValue;
                    var newValue = (ObservableCollection<UIElement>)eventArgs.NewValue;
    
                    if (oldValue != null)
                    {
                        oldValue.CollectionChanged -= instance.CollectionChanged_Children;
                    }
    
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += instance.CollectionChanged_Children;
                    }
    
                    instance.Changed_Children (oldValue, newValue);
                }
            }
    
            void CollectionChanged_Children(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionChanged_Children (
                    sender, 
                    e.Action,
                    e.OldStartingIndex,
                    e.OldItems,
                    e.NewStartingIndex,
                    e.NewItems
                    );
            }
    
            static object Coerce_Children (DependencyObject dependencyObject, object basevalue)
            {
                var instance = dependencyObject as AnimatedEntrance;
                if (instance == null)
                {
                    return basevalue;
                }
                var oldValue = (ObservableCollection<UIElement>)basevalue;
                var newValue = oldValue;
    
                instance.Coerce_Children (oldValue, ref newValue);
    
                if (newValue == null)
                {
                   newValue = new ObservableCollection<UIElement> ();
                }
    
                return newValue;
            }
    
            public static readonly DependencyProperty AnimationClockProperty = DependencyProperty.RegisterAttached (
                "AnimationClock",
                typeof (double),
                typeof (AnimatedEntrance),
                new FrameworkPropertyMetadata (
                    default (double),
                    FrameworkPropertyMetadataOptions.None,
                    Changed_AnimationClock,
                    Coerce_AnimationClock          
                ));
    
            static void Changed_AnimationClock (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                if (dependencyObject != null)
                {
                    var oldValue = (double)eventArgs.OldValue;
                    var newValue = (double)eventArgs.NewValue;
    
                    Changed_AnimationClock (dependencyObject, oldValue, newValue);
                }
            }
    
            static object Coerce_AnimationClock (DependencyObject dependencyObject, object basevalue)
            {
                if (dependencyObject == null)
                {
                    return basevalue;
                }
                var oldValue = (double)basevalue;
                var newValue = oldValue;
    
                Coerce_AnimationClock (dependencyObject, oldValue, ref newValue);
    
                return newValue;
            }
            #endregion
    
            // --------------------------------------------------------------------
            // Constructor
            // --------------------------------------------------------------------
            public AnimatedEntrance ()
            {
                CoerceAllProperties ();
                Constructed__AnimatedEntrance ();
            }
            // --------------------------------------------------------------------
            partial void Constructed__AnimatedEntrance ();
            // --------------------------------------------------------------------
            void CoerceAllProperties ()
            {
                CoerceValue (ChildrenProperty);
                CoerceValue (AnimationClockProperty);
            }
    
    
            // --------------------------------------------------------------------
            // Properties
            // --------------------------------------------------------------------
    
               
            // --------------------------------------------------------------------
            public ObservableCollection<UIElement> Children
            {
                get
                {
                    return (ObservableCollection<UIElement>)GetValue (ChildrenProperty);
                }
                private set
                {
                    if (Children != value)
                    {
                        SetValue (ChildrenPropertyKey, value);
                    }
                }
            }
            // --------------------------------------------------------------------
            partial void Changed_Children (ObservableCollection<UIElement> oldValue, ObservableCollection<UIElement> newValue);
            partial void Coerce_Children (ObservableCollection<UIElement> value, ref ObservableCollection<UIElement> coercedValue);
            partial void CollectionChanged_Children (object sender, NotifyCollectionChangedAction action, int oldStartingIndex, IList oldItems, int newStartingIndex, IList newItems);
            // --------------------------------------------------------------------
    
    
               
            // --------------------------------------------------------------------
            public static double GetAnimationClock (DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                {
                    return default (double);
                }
    
                return (double)dependencyObject.GetValue (AnimationClockProperty);
            }
    
            public static void SetAnimationClock (DependencyObject dependencyObject, double value)
            {
                if (dependencyObject != null)
                {
                    if (GetAnimationClock (dependencyObject) != value)
                    {
                        dependencyObject.SetValue (AnimationClockProperty, value);
                    }
                }
            }
    
            public static void ClearAnimationClock (DependencyObject dependencyObject)
            {
                if (dependencyObject != null)
                {
                    dependencyObject.ClearValue (AnimationClockProperty);
                }
            }
            // --------------------------------------------------------------------
            static partial void Changed_AnimationClock (DependencyObject dependencyObject, double oldValue, double newValue);
            static partial void Coerce_AnimationClock (DependencyObject dependencyObject, double value, ref double coercedValue);
            // --------------------------------------------------------------------
    
    
        }
        // ------------------------------------------------------------------------
    
    }
                                       
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_DependencyProperties.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_StateMachine.cs
namespace FileInclude
{
    using System.Windows;
    
    
    // ############################################################################
    // #                                                                          #
    // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
    // #                                                                          #
    // # This means that any edits to the .cs file will be lost when its          #
    // # regenerated. Changes should instead be applied to the corresponding      #
    // # template file (.tt)                                                      #
    // ############################################################################
    
    
    
    
    
    
    
    namespace Source.WPF
    {
        using System;
        using System.Threading;
    
        partial class AnimatedEntrance
        {
            abstract partial class BaseState
            {   
                public abstract State CurrentState {get;}
    
                // ----------------------------------------------------------------- 
                protected BaseState ()
                {
                    Constructed_BaseState();
                }
                // ----------------------------------------------------------------- 
                partial void Constructed_BaseState();
                // ----------------------------------------------------------------- 
    
                // ----------------------------------------------------------------- 
                public void InitializeState(BaseState state)
                {
                    OnInitializeState(state);
                }
    
                public void EnterState(BaseState previousState)
                {
                    OnEnterState(previousState);
                }
    
                public void LeaveState(BaseState nextState)
                {
                    OnLeaveState(nextState);
                }
    
                protected virtual void OnInitializeState(BaseState state)
                {
                    Initialize_BaseState(state);
                }
    
                protected virtual void OnEnterState(BaseState previousState)
                {
                    Enter_BaseState(previousState);
                }
    
                protected virtual void OnLeaveState(BaseState nextState)
                {
                    Leave_BaseState(nextState);
                }
                // ----------------------------------------------------------------- 
                partial void Initialize_BaseState(BaseState state);
                partial void Enter_BaseState(BaseState previousState);
                partial void Leave_BaseState(BaseState nextState);
                // ----------------------------------------------------------------- 
            }
    
    
            enum State
            {
                Initial,
                PresentingContent,
                Transitioning,
                DelayingNextTransition,
            }
    
            sealed partial class State_Initial : BaseState
            {
                // ----------------------------------------------------------------- 
                public State_Initial ()
                {
                    Constructed_State_Initial();
                }
                // ----------------------------------------------------------------- 
                partial void Constructed_State_Initial();
                // ----------------------------------------------------------------- 
    
                public override State CurrentState { get { return State.Initial; } }
    
                // ----------------------------------------------------------------- 
                protected override void OnEnterState(BaseState previousState)
                {
                    base.OnEnterState(previousState);
                    Enter_Initial(previousState);
                }
    
                protected override void OnLeaveState(BaseState nextState)
                {
                    Leave_Initial(nextState);
                    base.OnLeaveState(nextState);
                }
                // ----------------------------------------------------------------- 
                partial void Enter_Initial(BaseState previousState);
                partial void Leave_Initial(BaseState nextState);
                // ----------------------------------------------------------------- 
    
    
                // ----------------------------------------------------------------- 
                // From:    Initial
                // To:      PresentingContent
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_Initial_To_PresentingContent(UIElement current)
                {
                    var result = true;
    
                    IsEdgeValid_Initial_To_PresentingContent (current, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_Initial_To_PresentingContent(UIElement current)            
                {
                    if (!IsEdgeValid_Initial_To_PresentingContent(current))
                    {
                        return this;
                    }
    
                    var nextState = new State_PresentingContent();
    
                    InitializeState(nextState);
    
                    TransformInto_PresentingContent (current, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_Initial_To_PresentingContent (UIElement current, ref bool result);
                partial void TransformInto_PresentingContent (UIElement current, State_PresentingContent nextState);
                // ----------------------------------------------------------------- 
    
            }
    
            sealed partial class State_PresentingContent : BaseState
            {
                // ----------------------------------------------------------------- 
                public State_PresentingContent ()
                {
                    Constructed_State_PresentingContent();
                }
                // ----------------------------------------------------------------- 
                partial void Constructed_State_PresentingContent();
                // ----------------------------------------------------------------- 
    
                public override State CurrentState { get { return State.PresentingContent; } }
    
                // ----------------------------------------------------------------- 
                protected override void OnEnterState(BaseState previousState)
                {
                    base.OnEnterState(previousState);
                    Enter_PresentingContent(previousState);
                }
    
                protected override void OnLeaveState(BaseState nextState)
                {
                    Leave_PresentingContent(nextState);
                    base.OnLeaveState(nextState);
                }
                // ----------------------------------------------------------------- 
                partial void Enter_PresentingContent(BaseState previousState);
                partial void Leave_PresentingContent(BaseState nextState);
                // ----------------------------------------------------------------- 
    
    
                // ----------------------------------------------------------------- 
                // From:    PresentingContent
                // To:      Transitioning
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_PresentingContent_To_Transitioning(Option presentOption, UIElement next)
                {
                    var result = true;
    
                    IsEdgeValid_PresentingContent_To_Transitioning (presentOption, next, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_PresentingContent_To_Transitioning(Option presentOption, UIElement next)            
                {
                    if (!IsEdgeValid_PresentingContent_To_Transitioning(presentOption, next))
                    {
                        return this;
                    }
    
                    var nextState = new State_Transitioning();
    
                    InitializeState(nextState);
    
                    TransformInto_Transitioning (presentOption, next, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_PresentingContent_To_Transitioning (Option presentOption, UIElement next, ref bool result);
                partial void TransformInto_Transitioning (Option presentOption, UIElement next, State_Transitioning nextState);
                // ----------------------------------------------------------------- 
    
            }
    
            sealed partial class State_Transitioning : BaseState
            {
                // ----------------------------------------------------------------- 
                public State_Transitioning ()
                {
                    Constructed_State_Transitioning();
                }
                // ----------------------------------------------------------------- 
                partial void Constructed_State_Transitioning();
                // ----------------------------------------------------------------- 
    
                public override State CurrentState { get { return State.Transitioning; } }
    
                // ----------------------------------------------------------------- 
                protected override void OnEnterState(BaseState previousState)
                {
                    base.OnEnterState(previousState);
                    Enter_Transitioning(previousState);
                }
    
                protected override void OnLeaveState(BaseState nextState)
                {
                    Leave_Transitioning(nextState);
                    base.OnLeaveState(nextState);
                }
                // ----------------------------------------------------------------- 
                partial void Enter_Transitioning(BaseState previousState);
                partial void Leave_Transitioning(BaseState nextState);
                // ----------------------------------------------------------------- 
    
    
                // ----------------------------------------------------------------- 
                // From:    Transitioning
                // To:      PresentingContent
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_Transitioning_To_PresentingContent(UIElement current)
                {
                    var result = true;
    
                    IsEdgeValid_Transitioning_To_PresentingContent (current, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_Transitioning_To_PresentingContent(UIElement current)            
                {
                    if (!IsEdgeValid_Transitioning_To_PresentingContent(current))
                    {
                        return this;
                    }
    
                    var nextState = new State_PresentingContent();
    
                    InitializeState(nextState);
    
                    TransformInto_PresentingContent (current, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_Transitioning_To_PresentingContent (UIElement current, ref bool result);
                partial void TransformInto_PresentingContent (UIElement current, State_PresentingContent nextState);
                // ----------------------------------------------------------------- 
    
                // ----------------------------------------------------------------- 
                // From:    Transitioning
                // To:      DelayingNextTransition
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_Transitioning_To_DelayingNextTransition(UIElement current)
                {
                    var result = true;
    
                    IsEdgeValid_Transitioning_To_DelayingNextTransition (current, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_Transitioning_To_DelayingNextTransition(UIElement current)            
                {
                    if (!IsEdgeValid_Transitioning_To_DelayingNextTransition(current))
                    {
                        return this;
                    }
    
                    var nextState = new State_DelayingNextTransition();
    
                    InitializeState(nextState);
    
                    TransformInto_DelayingNextTransition (current, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_Transitioning_To_DelayingNextTransition (UIElement current, ref bool result);
                partial void TransformInto_DelayingNextTransition (UIElement current, State_DelayingNextTransition nextState);
                // ----------------------------------------------------------------- 
    
            }
    
            sealed partial class State_DelayingNextTransition : BaseState
            {
                // ----------------------------------------------------------------- 
                public State_DelayingNextTransition ()
                {
                    Constructed_State_DelayingNextTransition();
                }
                // ----------------------------------------------------------------- 
                partial void Constructed_State_DelayingNextTransition();
                // ----------------------------------------------------------------- 
    
                public override State CurrentState { get { return State.DelayingNextTransition; } }
    
                // ----------------------------------------------------------------- 
                protected override void OnEnterState(BaseState previousState)
                {
                    base.OnEnterState(previousState);
                    Enter_DelayingNextTransition(previousState);
                }
    
                protected override void OnLeaveState(BaseState nextState)
                {
                    Leave_DelayingNextTransition(nextState);
                    base.OnLeaveState(nextState);
                }
                // ----------------------------------------------------------------- 
                partial void Enter_DelayingNextTransition(BaseState previousState);
                partial void Leave_DelayingNextTransition(BaseState nextState);
                // ----------------------------------------------------------------- 
    
    
                // ----------------------------------------------------------------- 
                // From:    DelayingNextTransition
                // To:      Transitioning
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_DelayingNextTransition_To_Transitioning(Option presentOption, UIElement next)
                {
                    var result = true;
    
                    IsEdgeValid_DelayingNextTransition_To_Transitioning (presentOption, next, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_DelayingNextTransition_To_Transitioning(Option presentOption, UIElement next)            
                {
                    if (!IsEdgeValid_DelayingNextTransition_To_Transitioning(presentOption, next))
                    {
                        return this;
                    }
    
                    var nextState = new State_Transitioning();
    
                    InitializeState(nextState);
    
                    TransformInto_Transitioning (presentOption, next, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_DelayingNextTransition_To_Transitioning (Option presentOption, UIElement next, ref bool result);
                partial void TransformInto_Transitioning (Option presentOption, UIElement next, State_Transitioning nextState);
                // ----------------------------------------------------------------- 
    
                // ----------------------------------------------------------------- 
                // From:    DelayingNextTransition
                // To:      PresentingContent
                // ----------------------------------------------------------------- 
                public bool IsEdgeValid_DelayingNextTransition_To_PresentingContent(UIElement current)
                {
                    var result = true;
    
                    IsEdgeValid_DelayingNextTransition_To_PresentingContent (current, ref result);
    
                    return result;
                }
    
    
                public BaseState EdgeFrom_DelayingNextTransition_To_PresentingContent(UIElement current)            
                {
                    if (!IsEdgeValid_DelayingNextTransition_To_PresentingContent(current))
                    {
                        return this;
                    }
    
                    var nextState = new State_PresentingContent();
    
                    InitializeState(nextState);
    
                    TransformInto_PresentingContent (current, nextState);
    
                    LeaveState(nextState);
                    nextState.EnterState(this);
                   
                    return nextState;
                }
                // ----------------------------------------------------------------- 
                partial void IsEdgeValid_DelayingNextTransition_To_PresentingContent (UIElement current, ref bool result);
                partial void TransformInto_PresentingContent (UIElement current, State_PresentingContent nextState);
                // ----------------------------------------------------------------- 
    
            }
    
    
            abstract partial class BaseStateVisitor
            {
                public abstract BaseState Visit_Initial(State_Initial state);
                public abstract BaseState Visit_PresentingContent(State_PresentingContent state);
                public abstract BaseState Visit_Transitioning(State_Transitioning state);
                public abstract BaseState Visit_DelayingNextTransition(State_DelayingNextTransition state);
            }
    
            abstract partial class BaseThrowingStateVisitor : BaseStateVisitor
            {
                public override BaseState Visit_Initial(State_Initial state)
                {
                    throw new ArgumentException ("state");
                }
                public override BaseState Visit_PresentingContent(State_PresentingContent state)
                {
                    throw new ArgumentException ("state");
                }
                public override BaseState Visit_Transitioning(State_Transitioning state)
                {
                    throw new ArgumentException ("state");
                }
                public override BaseState Visit_DelayingNextTransition(State_DelayingNextTransition state)
                {
                    throw new ArgumentException ("state");
                }
            }
    
            abstract partial class BaseNoActionStateVisitor : BaseStateVisitor
            {
                public override BaseState Visit_Initial(State_Initial state)
                {
                    return state;
                }
                public override BaseState Visit_PresentingContent(State_PresentingContent state)
                {
                    return state;
                }
                public override BaseState Visit_Transitioning(State_Transitioning state)
                {
                    return state;
                }
                public override BaseState Visit_DelayingNextTransition(State_DelayingNextTransition state)
                {
                    return state;
                }
            }
    
            bool SetState (BaseState expectedState, BaseState nextState)
            {
                if (nextState == null)
                {
                    return false;
                }
    
                if (ReferenceEquals (expectedState, nextState))
                {
                    return true;
                }
    
                return ReferenceEquals(Interlocked.CompareExchange(ref m_currentState, nextState, expectedState),expectedState);
            }
    
            bool UpdateState(BaseStateVisitor visitor)
            {
                var currentState = m_currentState;
                if (visitor != null && currentState != null)
                {
                    switch (currentState.CurrentState)
                    {
                    case State.Initial:
                        return SetState (currentState, visitor.Visit_Initial((State_Initial)currentState));
                    case State.PresentingContent:
                        return SetState (currentState, visitor.Visit_PresentingContent((State_PresentingContent)currentState));
                    case State.Transitioning:
                        return SetState (currentState, visitor.Visit_Transitioning((State_Transitioning)currentState));
                    case State.DelayingNextTransition:
                        return SetState (currentState, visitor.Visit_DelayingNextTransition((State_DelayingNextTransition)currentState));
                    default:
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
    
            BaseState m_currentState;
        }
    }
    
                                       
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_StateMachine.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\Extensions\WpfExtensions.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    
    
    
    namespace Source.Extensions
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Windows;
        using System.Windows.Data;
        using System.Windows.Media;
        using System.Windows.Threading;
    
        using Source.Common;
    
        static partial class WpfExtensions
        {
            public static void Async_Invoke (
                this Dispatcher dispatcher, 
                string actionName, 
                Action action
                )
            {
                if (action == null)
                {
                    return;
                }
    
                Action act = () =>
                                 {
    #if DEBUG
                                     Log.Info ("Async_Invoke: {0}", actionName ?? "Unknown");
    #endif
    
                                     try
                                     {
                                         action ();
                                     }
                                     catch (Exception exc)
                                     {
                                         Log.Exception ("Async_Invoke: Caught exception: {0}", exc);
                                     }
                                 };
    
                dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
                dispatcher.BeginInvoke (DispatcherPriority.ApplicationIdle, act);
            }
    
            public static void Async_Invoke (
                this DependencyObject dependencyObject, 
                string actionName, 
                Action action
                )
            {
                var dispatcher = dependencyObject == null ? Dispatcher.CurrentDispatcher : dependencyObject.Dispatcher;
    
                dispatcher.Async_Invoke (actionName, action);
            }
    
            public static BindingBase GetBindingOf (
                this DependencyObject dependencyObject, 
                DependencyProperty dependencyProperty 
                )
            {
                if (dependencyObject == null)
                {
                    return null;
                }
    
                if (dependencyProperty == null)
                {
                    return null;
                }
    
                return BindingOperations.GetBindingBase (dependencyObject, dependencyProperty);
            }
    
            public static bool IsBoundTo (
                this DependencyObject dependencyObject, 
                DependencyProperty dependencyProperty 
                )
            {
                if (dependencyObject == null)
                {
                    return false;
                }
    
                if (dependencyProperty == null)
                {
                    return false;
                }
    
                return BindingOperations.IsDataBound (dependencyObject, dependencyProperty);
            }
    
            public static void ResetBindingOf (
                this DependencyObject dependencyObject, 
                DependencyProperty dependencyProperty, 
                BindingBase binding = null
                )
            {
                if (dependencyObject == null)
                {
                    return;
                }
    
                if (dependencyProperty == null)
                {
                    return;
                }
    
                BindingOperations.ClearBinding (dependencyObject, dependencyProperty);
    
                if (binding != null)
                {
                    BindingOperations.SetBinding (dependencyObject, dependencyProperty, binding);
                }
            }
    
            public static TFreezable FreezeObject<TFreezable> (this TFreezable freezable)
                where TFreezable : Freezable
            {
                if (freezable == null)
                {
                    return null;
                }
    
                if (!freezable.IsFrozen && freezable.CanFreeze)
                {
                    freezable.Freeze ();
                }
    
                return freezable;
            }
    
            public static TranslateTransform ToTranslateTransform (this Vector v)
            {
                return new TranslateTransform (v.X, v.Y);
            }
    
            public static TranslateTransform UpdateFromVector (this TranslateTransform tt, Vector v)
            {
                if (tt == null)
                {
                    return null;
                }
    
                tt.X = v.X;
                tt.Y = v.Y;
    
                return tt;
            }
    
            public static bool IsNear (this double v, double c)
            {
                return Math.Abs(v - c) < double.Epsilon * 100;            
            }
    
            public static double Interpolate (this double t, double from, double to)
            {
                if (t < 0)
                {
                    return from;
                }
    
                if (t > 1)
                {
                    return to;
                }
    
                return t*(to - from) + from;
            }
            
            public static Vector Interpolate (this double t, Vector from, Vector to)
            {
                if (t < 0)
                {
                    return from;
                }
    
                if (t > 1)
                {
                    return to;
                }
    
                return new Vector (
                    t*(to.X - from.X) + from.X,
                    t*(to.Y - from.Y) + from.Y
                    );
            }
    
            public static Rect ToRect (this Size size)
            {
                return new Rect(0,0,size.Width, size.Height);
            }
        
            public static IEnumerable<DependencyObject> GetVisualChildren (this DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                {
                    yield break;
                }
    
                var count = VisualTreeHelper.GetChildrenCount (dependencyObject);
                for (var iter = 0; iter < count; ++iter)
                {
                    yield return VisualTreeHelper.GetChild (dependencyObject, iter);
                }
            }
    
            public static IEnumerable<DependencyObject> GetLogicalChildren (this DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                {
                    return Array<DependencyObject>.Empty;    
                }
    
                return LogicalTreeHelper.GetChildren (dependencyObject).OfType<DependencyObject> ();
            }
        }
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\Extensions\WpfExtensions.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AccordionPanel_DependencyProperties.cs
namespace FileInclude
{
    
    // ############################################################################
    // #                                                                          #
    // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
    // #                                                                          #
    // # This means that any edits to the .cs file will be lost when its          #
    // # regenerated. Changes should instead be applied to the corresponding      #
    // # template file (.tt)                                                      #
    // ############################################################################
    
    
    
                                       
    
    
    namespace Source.WPF
    {
        using System.Collections;
        using System.Collections.ObjectModel;
        using System.Collections.Specialized;
    
        using System.Windows;
        using System.Windows.Media;
    
        // ------------------------------------------------------------------------
        // AccordionPanel
        // ------------------------------------------------------------------------
        partial class AccordionPanel
        {
            #region Uninteresting generated code
            public static readonly DependencyProperty PreviewWidthProperty = DependencyProperty.Register (
                "PreviewWidth",
                typeof (double),
                typeof (AccordionPanel),
                new FrameworkPropertyMetadata (
                    32.0,
                    FrameworkPropertyMetadataOptions.None,
                    Changed_PreviewWidth,
                    Coerce_PreviewWidth          
                ));
    
            static void Changed_PreviewWidth (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                var instance = dependencyObject as AccordionPanel;
                if (instance != null)
                {
                    var oldValue = (double)eventArgs.OldValue;
                    var newValue = (double)eventArgs.NewValue;
    
                    instance.Changed_PreviewWidth (oldValue, newValue);
                }
            }
    
    
            static object Coerce_PreviewWidth (DependencyObject dependencyObject, object basevalue)
            {
                var instance = dependencyObject as AccordionPanel;
                if (instance == null)
                {
                    return basevalue;
                }
                var oldValue = (double)basevalue;
                var newValue = oldValue;
    
                instance.Coerce_PreviewWidth (oldValue, ref newValue);
    
    
                return newValue;
            }
    
            public static readonly DependencyProperty ActiveElementProperty = DependencyProperty.Register (
                "ActiveElement",
                typeof (UIElement),
                typeof (AccordionPanel),
                new FrameworkPropertyMetadata (
                    default (UIElement),
                    FrameworkPropertyMetadataOptions.None,
                    Changed_ActiveElement,
                    Coerce_ActiveElement          
                ));
    
            static void Changed_ActiveElement (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                var instance = dependencyObject as AccordionPanel;
                if (instance != null)
                {
                    var oldValue = (UIElement)eventArgs.OldValue;
                    var newValue = (UIElement)eventArgs.NewValue;
    
                    instance.Changed_ActiveElement (oldValue, newValue);
                }
            }
    
    
            static object Coerce_ActiveElement (DependencyObject dependencyObject, object basevalue)
            {
                var instance = dependencyObject as AccordionPanel;
                if (instance == null)
                {
                    return basevalue;
                }
                var oldValue = (UIElement)basevalue;
                var newValue = oldValue;
    
                instance.Coerce_ActiveElement (oldValue, ref newValue);
    
    
                return newValue;
            }
    
            public static readonly DependencyProperty ChildStateProperty = DependencyProperty.RegisterAttached (
                "ChildState",
                typeof (AccordionPanel.State),
                typeof (AccordionPanel),
                new FrameworkPropertyMetadata (
                    default (AccordionPanel.State),
                    FrameworkPropertyMetadataOptions.None,
                    Changed_ChildState,
                    Coerce_ChildState          
                ));
    
            static void Changed_ChildState (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                if (dependencyObject != null)
                {
                    var oldValue = (AccordionPanel.State)eventArgs.OldValue;
                    var newValue = (AccordionPanel.State)eventArgs.NewValue;
    
                    Changed_ChildState (dependencyObject, oldValue, newValue);
                }
            }
    
            static object Coerce_ChildState (DependencyObject dependencyObject, object basevalue)
            {
                if (dependencyObject == null)
                {
                    return basevalue;
                }
                var oldValue = (AccordionPanel.State)basevalue;
                var newValue = oldValue;
    
                Coerce_ChildState (dependencyObject, oldValue, ref newValue);
    
                return newValue;
            }
            public static readonly DependencyProperty AnimationClockProperty = DependencyProperty.RegisterAttached (
                "AnimationClock",
                typeof (double),
                typeof (AccordionPanel),
                new FrameworkPropertyMetadata (
                    default (double),
                    FrameworkPropertyMetadataOptions.None,
                    Changed_AnimationClock,
                    Coerce_AnimationClock          
                ));
    
            static void Changed_AnimationClock (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
            {
                if (dependencyObject != null)
                {
                    var oldValue = (double)eventArgs.OldValue;
                    var newValue = (double)eventArgs.NewValue;
    
                    Changed_AnimationClock (dependencyObject, oldValue, newValue);
                }
            }
    
            static object Coerce_AnimationClock (DependencyObject dependencyObject, object basevalue)
            {
                if (dependencyObject == null)
                {
                    return basevalue;
                }
                var oldValue = (double)basevalue;
                var newValue = oldValue;
    
                Coerce_AnimationClock (dependencyObject, oldValue, ref newValue);
    
                return newValue;
            }
            #endregion
    
            // --------------------------------------------------------------------
            // Constructor
            // --------------------------------------------------------------------
            public AccordionPanel ()
            {
                CoerceAllProperties ();
                Constructed__AccordionPanel ();
            }
            // --------------------------------------------------------------------
            partial void Constructed__AccordionPanel ();
            // --------------------------------------------------------------------
            void CoerceAllProperties ()
            {
                CoerceValue (PreviewWidthProperty);
                CoerceValue (ActiveElementProperty);
                CoerceValue (ChildStateProperty);
                CoerceValue (AnimationClockProperty);
            }
    
    
            // --------------------------------------------------------------------
            // Properties
            // --------------------------------------------------------------------
    
               
            // --------------------------------------------------------------------
            public double PreviewWidth
            {
                get
                {
                    return (double)GetValue (PreviewWidthProperty);
                }
                set
                {
                    if (PreviewWidth != value)
                    {
                        SetValue (PreviewWidthProperty, value);
                    }
                }
            }
            // --------------------------------------------------------------------
            partial void Changed_PreviewWidth (double oldValue, double newValue);
            partial void Coerce_PreviewWidth (double value, ref double coercedValue);
            // --------------------------------------------------------------------
    
    
               
            // --------------------------------------------------------------------
            public UIElement ActiveElement
            {
                get
                {
                    return (UIElement)GetValue (ActiveElementProperty);
                }
                set
                {
                    if (ActiveElement != value)
                    {
                        SetValue (ActiveElementProperty, value);
                    }
                }
            }
            // --------------------------------------------------------------------
            partial void Changed_ActiveElement (UIElement oldValue, UIElement newValue);
            partial void Coerce_ActiveElement (UIElement value, ref UIElement coercedValue);
            // --------------------------------------------------------------------
    
    
               
            // --------------------------------------------------------------------
            public static AccordionPanel.State GetChildState (DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                {
                    return default (AccordionPanel.State);
                }
    
                return (AccordionPanel.State)dependencyObject.GetValue (ChildStateProperty);
            }
    
            public static void SetChildState (DependencyObject dependencyObject, AccordionPanel.State value)
            {
                if (dependencyObject != null)
                {
                    if (GetChildState (dependencyObject) != value)
                    {
                        dependencyObject.SetValue (ChildStateProperty, value);
                    }
                }
            }
    
            public static void ClearChildState (DependencyObject dependencyObject)
            {
                if (dependencyObject != null)
                {
                    dependencyObject.ClearValue (ChildStateProperty);
                }
            }
            // --------------------------------------------------------------------
            static partial void Changed_ChildState (DependencyObject dependencyObject, AccordionPanel.State oldValue, AccordionPanel.State newValue);
            static partial void Coerce_ChildState (DependencyObject dependencyObject, AccordionPanel.State value, ref AccordionPanel.State coercedValue);
            // --------------------------------------------------------------------
    
    
               
            // --------------------------------------------------------------------
            public static double GetAnimationClock (DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                {
                    return default (double);
                }
    
                return (double)dependencyObject.GetValue (AnimationClockProperty);
            }
    
            public static void SetAnimationClock (DependencyObject dependencyObject, double value)
            {
                if (dependencyObject != null)
                {
                    if (GetAnimationClock (dependencyObject) != value)
                    {
                        dependencyObject.SetValue (AnimationClockProperty, value);
                    }
                }
            }
    
            public static void ClearAnimationClock (DependencyObject dependencyObject)
            {
                if (dependencyObject != null)
                {
                    dependencyObject.ClearValue (AnimationClockProperty);
                }
            }
            // --------------------------------------------------------------------
            static partial void Changed_AnimationClock (DependencyObject dependencyObject, double oldValue, double newValue);
            static partial void Coerce_AnimationClock (DependencyObject dependencyObject, double value, ref double coercedValue);
            // --------------------------------------------------------------------
    
    
        }
        // ------------------------------------------------------------------------
    
    }
                                       
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\WPF\Generated_AccordionPanel_DependencyProperties.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\Common\Array.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    namespace Source.Common
    {
        static class Array<T>
        {
            public static readonly T[] Empty = new T[0];
        }
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\Common\Array.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\Common\Log.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    
    
    namespace Source.Common
    {
        using System;
        using System.Globalization;
    
        static partial class Log
        {
            static partial void Partial_LogLevel (Level level);
            static partial void Partial_LogMessage (Level level, string message);
            static partial void Partial_ExceptionOnLog (Level level, string format, object[] args, Exception exc);
    
            public static void LogMessage (Level level, string format, params object[] args)
            {
                try
                {
                    Partial_LogLevel (level);
                    Partial_LogMessage (level, GetMessage (format, args));
                }
                catch (Exception exc)
                {
                    Partial_ExceptionOnLog (level, format, args, exc);
                }
                
            }
    
            static string GetMessage (string format, object[] args)
            {
                format = format ?? "";
                try
                {
                    return (args == null || args.Length == 0)
                               ? format
                               : string.Format (Config.DefaultCulture, format, args)
                        ;
                }
                catch (FormatException)
                {
    
                    return format;
                }
            }
        }
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\Common\Log.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\Common\Config.cs
namespace FileInclude
{
    // ----------------------------------------------------------------------------------------------
    // Copyright (c) Mårten Rånge.
    // ----------------------------------------------------------------------------------------------
    // This source code is subject to terms and conditions of the Microsoft Public License. A 
    // copy of the license can be found in the License.html file at the root of this distribution. 
    // If you cannot locate the  Microsoft Public License, please send an email to 
    // dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
    //  by the terms of the Microsoft Public License.
    // ----------------------------------------------------------------------------------------------
    // You must not remove this notice, or any other, from this software.
    // ----------------------------------------------------------------------------------------------
    
    
    namespace Source.Common
    {
        using System.Globalization;
    
        sealed partial class InitConfig
        {
            public CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
        }
    
        static partial class Config
        {
            static partial void Partial_Constructed(ref InitConfig initConfig);
    
            public readonly static CultureInfo DefaultCulture;
    
            static Config ()
            {
                var initConfig = new InitConfig();
    
                Partial_Constructed (ref initConfig);
    
                initConfig = initConfig ?? new InitConfig();
    
                DefaultCulture = initConfig.DefaultCulture;
            }
        }
    }
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\Common\Config.cs
// ############################################################################

// ############################################################################
// @@@ BEGIN_INCLUDE: C:\temp\GitHub\T4Include\Common\Generated_Log.cs
namespace FileInclude
{
    // ############################################################################
    // #                                                                          #
    // #        ---==>  T H I S  F I L E  I S   G E N E R A T E D  <==---         #
    // #                                                                          #
    // # This means that any edits to the .cs file will be lost when its          #
    // # regenerated. Changes should instead be applied to the corresponding      #
    // # template file (.tt)                                                      #
    // ############################################################################
    
    
    
    
    
    
    namespace Source.Common
    {
        using System;
    
        partial class Log
        {
            public enum Level
            {
                Success = 1000,
                HighLight = 2000,
                Info = 3000,
                Warning = 10000,
                Error = 20000,
                Exception = 21000,
            }
    
            public static void Success (string format, params object[] args)
            {
                LogMessage (Level.Success, format, args);
            }
            public static void HighLight (string format, params object[] args)
            {
                LogMessage (Level.HighLight, format, args);
            }
            public static void Info (string format, params object[] args)
            {
                LogMessage (Level.Info, format, args);
            }
            public static void Warning (string format, params object[] args)
            {
                LogMessage (Level.Warning, format, args);
            }
            public static void Error (string format, params object[] args)
            {
                LogMessage (Level.Error, format, args);
            }
            public static void Exception (string format, params object[] args)
            {
                LogMessage (Level.Exception, format, args);
            }
    #if !NETFX_CORE && !SILVERLIGHT && !WINDOWS_PHONE
            static ConsoleColor GetLevelColor (Level level)
            {
                switch (level)
                {
                    case Level.Success:
                        return ConsoleColor.Green;
                    case Level.HighLight:
                        return ConsoleColor.White;
                    case Level.Info:
                        return ConsoleColor.Gray;
                    case Level.Warning:
                        return ConsoleColor.Yellow;
                    case Level.Error:
                        return ConsoleColor.Red;
                    case Level.Exception:
                        return ConsoleColor.Red;
                    default:
                        return ConsoleColor.Magenta;
                }
            }
    #endif
            static string GetLevelMessage (Level level)
            {
                switch (level)
                {
                    case Level.Success:
                        return "SUCCESS  ";
                    case Level.HighLight:
                        return "HIGHLIGHT";
                    case Level.Info:
                        return "INFO     ";
                    case Level.Warning:
                        return "WARNING  ";
                    case Level.Error:
                        return "ERROR    ";
                    case Level.Exception:
                        return "EXCEPTION";
                    default:
                        return "UNKNOWN  ";
                }
            }
    
        }
    }
    
}
// @@@ END_INCLUDE: C:\temp\GitHub\T4Include\Common\Generated_Log.cs
// ############################################################################

// ############################################################################
namespace FileInclude.Include
{
    static partial class MetaData
    {
        public const string RootPath        = @"..\..\..";
        public const string IncludeDate     = @"2013-03-29T07:39:56";

        public const string Include_0       = @"C:\temp\GitHub\T4Include\WPF\AnimatedEntrance.cs";
        public const string Include_1       = @"C:\temp\GitHub\T4Include\WPF\AccordionPanel.cs";
        public const string Include_2       = @"C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_DependencyProperties.cs";
        public const string Include_3       = @"C:\temp\GitHub\T4Include\WPF\Generated_AnimatedEntrance_StateMachine.cs";
        public const string Include_4       = @"C:\temp\GitHub\T4Include\Extensions\WpfExtensions.cs";
        public const string Include_5       = @"C:\temp\GitHub\T4Include\WPF\Generated_AccordionPanel_DependencyProperties.cs";
        public const string Include_6       = @"C:\temp\GitHub\T4Include\Common\Array.cs";
        public const string Include_7       = @"C:\temp\GitHub\T4Include\Common\Log.cs";
        public const string Include_8       = @"C:\temp\GitHub\T4Include\Common\Config.cs";
        public const string Include_9       = @"C:\temp\GitHub\T4Include\Common\Generated_Log.cs";
    }
}
// ############################################################################


