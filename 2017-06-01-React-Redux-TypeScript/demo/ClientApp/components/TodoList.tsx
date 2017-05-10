import * as React from 'react';
import * as TodoStore from '../store/TodoStore';
import Todo from './Todo';
import NewTodo from './NewTodo';

interface ITodoListProps
{
    todos: TodoStore.TodoItem[];
    addTodo: (subject: string) => void;
    deleteTodo: (id: number) => void;
}

export default class TodoList extends React.Component<ITodoListProps, {}>
{
    constructor()
    {
        super();
    }

    private renderTodoItem(item:TodoStore.TodoItem)
    {
        const { deleteTodo } = this.props;
        return (<Todo 
                    item={item} 
                    deleteTodo={deleteTodo} 
                />);
    }

    private renderTodoItems()
    {
        const { todos } = this.props;
        var todoList = todos.map((item)=> { 
            return this.renderTodoItem(item);
        });
        return todoList;
    }

    public render(){
        return (
                <div className="panel panel-default">
                    <div className="panel-heading">To Do List</div>
                    <div className="row">
                        <div className="col-md-2">
                        Add todo: 
                        </div>
                        <div className="col-md-10">
                            <NewTodo addTodo={this.props.addTodo} />
                        </div>
                    </div>
                    <ul className="list-group">
                        {this.renderTodoItems()}
                    </ul>
                </div>   
        );
    }
}
