import { Dispatch, createContext, useContext, useReducer } from "react";
import { Action, State, initState, reducer } from "../reducers/DataReducer";
import React from "react";

interface DataContextProps {
    data: State,
    dispatch: Dispatch<Action>
}

export const DataContext = createContext<DataContextProps | null>(null)
export const UseData = () => useContext(DataContext)
type Props = {
    children?: React.ReactNode
};
const DataProvider: React.FC<Props> = ({children}) => {
    const [data, dispatch] = useReducer(reducer, initState)

    return (
        <DataContext.Provider value={{ data, dispatch }}>{children}</DataContext.Provider>
    )
}

export default DataProvider