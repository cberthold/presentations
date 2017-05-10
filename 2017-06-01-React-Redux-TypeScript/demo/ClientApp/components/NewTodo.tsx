import * as React from 'react';

interface IState
{
    whatsNext: string;
}

interface INewTodoProps
{
    addTodo(subject: string): void;
}

export default class NewTodo extends React.Component<INewTodoProps, IState>
{
    constructor()
    {
        super();
        this.state = { whatsNext: ''};
        this.onPropertyChange = this.onPropertyChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    private onPropertyChange(e:React.FormEvent<HTMLInputElement>) : void
    {
        const text = e.currentTarget.value;
        this.setState({ whatsNext: text });
    }

    private handleSubmit(e: React.KeyboardEvent<HTMLInputElement>) : void
    {   
        if(e.which === 13)
        {
            this.props.addTodo(this.state.whatsNext);
            this.setState({ whatsNext: ''});
        }
    }


    public render()
    {
        return (
            <input type="text" 
                value={this.state.whatsNext} 
                onChange={ this.onPropertyChange } 
                onKeyDown={ this.handleSubmit } />
        );

    }
}