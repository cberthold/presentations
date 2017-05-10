import * as React from 'react';
import * as TodoStore from '../store/TodoStore';


interface ITodoItemProps
{
    item: TodoStore.TodoItem;
    deleteTodo: (id:number) => void;
}

export default class Todo extends React.Component<ITodoItemProps, {}>
{
    constructor()
    {
        super();
    }

    private doubleClick():void
    {
        const { item, deleteTodo } = this.props;
        deleteTodo(item.id);
    }

    public render()
    {
        const { item } = this.props;
        return (<li className="list-group-item" onDoubleClick={()=> this.doubleClick()}>{item.subject}</li>);
    }
}