import { useEffect } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, Tooltip, OverlayTrigger } from "react-bootstrap";
import moment from 'moment';
import SearchMember from "../components/SearchMember";
import AddMatch from "../components/AddMatch";
import { selectState, onChangePageIndex, onChangePageSize, fetchAsync, onShowAdd, addAsync, MatchProps, deleteAsync, onShowDelete, onSelectedId, onCloseError, updateAsync, onShowUpdate, onShowTeamRival, rollingAsync } from '../slices/matchSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { NavLink } from "react-router-dom";
import Confirmation from "../components/Confirmation";
import UpdateMatch from "../components/UpdateMatch";
import Rivals from "../components/Rivals";
import { FaEdit, FaFutbol, FaPlus, FaTrash, FaUsers } from "react-icons/fa";

export default function Match() {
    let sizes = [10, 50, 100]
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize }))
            .unwrap()
            .then(values => {

            })
            .catch(error => {

            })
    }

    const addMatch = (match: MatchProps) => {
        dispatch(onShowAdd(false))
        dispatch(addAsync(match)).unwrap()
            .then(values => dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize })).unwrap())
            .catch(error => {

            })
    }

    const onDeleteEvent = (id: number) => {
        dispatch(onShowDelete(true))
        dispatch(onSelectedId(id))
    }

    const onTeamRivalEvent = (id: number) => {
        dispatch(onSelectedId(id))
        dispatch(rollingAsync({ id: id })).unwrap()
            .then(value => dispatch(onShowTeamRival(true)))
            .catch(error => {

            })
    }

    const deleteMatch = (id: number) => {
        dispatch(onShowDelete(false))
        dispatch(deleteAsync(id)).unwrap()
            .then(values => dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize })).unwrap())
            .catch(err => { console.log('ui', err) })

    }

    const updateMatch = (data: MatchProps) => {
        dispatch(onShowUpdate(false))
        dispatch(updateAsync(data)).unwrap()
            .then(values => dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize })).unwrap())
            .catch(err => { console.log('ui', err) })

    }

    useEffect(() => {
        console.log('effect', state)
        fetch()
        return function cleanup() {
            console.log('unmount', state)
        }

    }, [state.pageIndex, state.pageSize])
    return (
        <>
            <h1>Matches</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                    {state.error}
                </Alert>
            }
            {
                state.isShowAdd &&
                <AddMatch onSubmit={(match) => { addMatch(match) }} show={state.isShowAdd} onClose={() => { dispatch(onShowAdd(false)) }} />
            }
            {
                state.isShowRivals && state.rolling &&
                <Rivals onSubmit={(rivals) => { }} show={state.isShowRivals} rivals={state.rolling} match={state.data.find(x => x.id === state.selectedId)} onClose={() => dispatch(onShowTeamRival(false))} />
            }
            {
                state.isShowUpdate &&
                <UpdateMatch initData={state.data.find(x => x.id === state.selectedId)} onSubmit={(match) => { updateMatch(match) }} show={state.isShowUpdate} onClose={() => { dispatch(onShowUpdate(false)) }} />
            }
            {
                state.isShowDelete &&
                <Confirmation
                    show={state.isShowDelete}
                    title={"Confirmation"}
                    content={"Are you sure you want to permanently delete this member?"}
                    onSubmit={() => deleteMatch(state.selectedId)}
                    onClose={() => dispatch(onShowDelete(false))} />
            }
            <SearchMember onSearchChanged={() => { }} onSubmit={() => { }} />

            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <OverlayTrigger overlay={<Tooltip>Add match</Tooltip>}>
                        <Button variant="primary" onClick={() => { dispatch(onShowAdd(true)) }}><FaPlus /></Button>
                    </OverlayTrigger>
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Team count</th>
                        <th>Team size</th>
                        <th>Modified Date</th>
                        <th className="col-sm-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(value =>
                            <tr key={`key-${value.id}`}>
                                <td>{value.id}</td>
                                <td>{value.name}</td>
                                <td>{value.description}</td>
                                <td>{value.teamCount}</td>
                                <td>{value.teamSize}</td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <NavLink to={`${value.id}`}>
                                        <OverlayTrigger overlay={<Tooltip>Member</Tooltip>}>
                                            <Button className="me-2" variant="primary" onClick={() => { }}><FaUsers /></Button>
                                        </OverlayTrigger>
                                    </NavLink>
                                    <OverlayTrigger overlay={<Tooltip>Edit</Tooltip>}>
                                        <Button variant="warning" onClick={() => {
                                            dispatch(onSelectedId(value.id))
                                            dispatch(onShowUpdate(true))
                                        }}><FaEdit /></Button>
                                    </OverlayTrigger>
                                    <OverlayTrigger overlay={<Tooltip>Delete</Tooltip>}>
                                        <Button className="mx-2" variant="danger" onClick={() => { onDeleteEvent(value.id) }}><FaTrash /></Button>
                                    </OverlayTrigger>
                                    <OverlayTrigger overlay={<Tooltip>Team Division Rivals</Tooltip>}>
                                        <Button variant="success" disabled={!value.hasTeamRival} onClick={() => { onTeamRivalEvent(value.id) }}><FaFutbol /></Button>
                                    </OverlayTrigger>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
            {
                state.totalPage &&
                <>
                    <Row>
                        <Col>
                            <DropdownButton className="d-inline" size="lg" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
                                {
                                    sizes.map(element =>
                                        <Dropdown.Item key={`ditem-${element}`} onClick={() => dispatch(onChangePageSize(element))}>{element}</Dropdown.Item>
                                    )
                                }
                            </DropdownButton> <p className="d-inline">Records Page {state.pageIndex + 1} of {state.totalPage}</p></Col>
                        <Col>
                            <Pagination className="d-flex justify-content-end">
                                {
                                    Array.from(Array(state.totalPage).keys()).map(element => {
                                        var pageNumber = element + 1
                                        return <Pagination.Item
                                            onClick={() => dispatch(onChangePageIndex(pageNumber - 1))}
                                            key={`pitem-${element}`}
                                            active={state.pageIndex === pageNumber - 1}>{pageNumber}</Pagination.Item>
                                    })
                                }
                            </Pagination>
                        </Col>
                    </Row>
                </>
            }

        </>
    )
}