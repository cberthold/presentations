import { Action, Reducer } from 'redux';

export interface TodoItem {
    id: number;
    subject: string;
}

export interface TodoContainerState
{
    lastId: number;
    todos: TodoItem[];
}

interface AddTodoAction { type: 'ADD_TODO', subject: string; };
interface DeleteTodoAction { type: 'DELETE_TODO', id: number };

type KnownAction = AddTodoAction | DeleteTodoAction;

export const actionCreators = {
    addTodo: (subject: string) => <AddTodoAction>{ type: 'ADD_TODO', subject },
    deleteTodo: (id: number) => <DeleteTodoAction>{ type: 'DELETE_TODO', id }
};

const getNextId = (currentState: TodoContainerState) : number => {
    return currentState.lastId + 1;
}


const createTodoItem = (state: TodoContainerState, action: AddTodoAction) : TodoContainerState => {
    var nextId = getNextId(state);
    
    const item:TodoItem = {
        id: nextId,
        subject: action.subject,
    };
    return Object.assign({}, state, { lastId: nextId, todos: [...state.todos, nextId]});
}

const removeTodoItem = (state: TodoContainerState, action: DeleteTodoAction) : TodoContainerState => {
    const removedTodoItem = state.todos.filter((item)=> item.id !== action.id);

    return Object.assign({}, state, { todos: removedTodoItem});
}

export const reducer: Reducer<TodoContainerState> = (state: TodoContainerState, action: KnownAction) => {
    switch (action.type) {
        case 'ADD_TODO':
            return createTodoItem(state, action);
        case 'DELETE_TODO':
            return removeTodoItem(state, action);
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    // For unrecognized actions (or in cases where actions have no effect), must return the existing state
    //  (or default initial state if none was supplied)
    return state || { lastId: 0, todos: [] };
};

