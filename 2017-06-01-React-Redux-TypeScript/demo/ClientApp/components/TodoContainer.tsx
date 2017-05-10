import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as TodoStore from '../store/TodoStore';
import TodoList from './TodoList';


type TodoContainerProps = TodoStore.TodoContainerState & typeof TodoStore.actionCreators;

class TodoContainer extends React.Component<TodoContainerProps, {}>
{
    constructor()
    {
        super();
    }

    public render()
    {
        const { addTodo, deleteTodo, todos } = this.props;
        return (<TodoList 
                    addTodo={addTodo} 
                    deleteTodo={deleteTodo} 
                    todos={todos}   
                />);
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.todo, // Selects which state properties are merged into the component's props
    TodoStore.actionCreators                 // Selects which action creators are merged into the component's props
)(TodoContainer);